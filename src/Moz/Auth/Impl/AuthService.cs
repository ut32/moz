using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Auth;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Common;
using Moz.Bus.Models.Members;
using Moz.Bus.Services;
using Moz.Bus.Services.Members;
using Moz.Core.Options;
using Moz.DataBase;
using Moz.Exceptions;
using Moz.Service.Security;
using Moz.WebApi;

namespace Moz.Auth.Impl
{
    internal class AuthService : BaseService,IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemberService _memberService;
        private readonly IEncryptionService _encryptionService;
        private readonly IOptions<MozOptions> _mozOptions;
        private readonly IDistributedCache _distributedCache;
        private readonly IJwtService _jwtService;

        public AuthService(IHttpContextAccessor httpContextAccessor,
            IMemberService memberService,
            IEncryptionService encryptionService,
            IOptions<MozOptions> mozOptions,
            IDistributedCache distributedCache, IJwtService jwtService)
        { 
            _httpContextAccessor = httpContextAccessor;
            _memberService = memberService;
            _encryptionService = encryptionService;
            _mozOptions = mozOptions;
            _distributedCache = distributedCache;
            _jwtService = jwtService;
        }
        
        #region Utils
        private bool PasswordsMatch(string passwordInDb, string salt, string enteredPassword)
        {
            if (string.IsNullOrEmpty(passwordInDb) ||
                string.IsNullOrEmpty(salt) ||
                string.IsNullOrEmpty(enteredPassword))
                return false;
            var savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, salt,"SHA512");
            return passwordInDb.Equals(savedPassword, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServResult<string> GetAuthenticatedUId()
        {
            var result = _httpContextAccessor 
                .HttpContext
                .AuthenticateAsync(MozAuthAttribute.MozAuthorizeSchemes)
                .GetAwaiter()
                .GetResult();
            if (!result?.Principal?.Identity?.IsAuthenticated??false) 
                return Error("未登录",401);
            
            var claim = result?.Principal?.Claims?.FirstOrDefault(o => o.Type == "jti");
            if (claim == null || claim.Value.IsNullOrEmpty()) 
                return Error("未登录",401);

            return claim.Value;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServResult<SimpleMember> GetAuthenticatedMember()
        {
            var result = GetAuthenticatedUId();
            if (result.Code>0)
                return Error(result.Message,result.Code);

            var member = _memberService.GetSimpleMemberByUId(result.Data);
            
            if (member == null)
                return Error("找不到用户",404);
            if (!member.IsActive)
                return Error("用户未激活");
            if (member.IsDelete) 
                return Error("用户已删除");
            if (member.CannotLoginUntilDate.HasValue && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
                return Error("用户已被关小黑屋");

            return member;
        }

        public bool AddRoleToMemberId(long memberId, long roleId, DateTime? expDatetime=null) 
        { 
            using (var client = DbFactory.GetClient())
            { 
                var mRole = client.Queryable<MemberRole>()
                    .Single(t => t.MemberId == memberId && t.RoleId == roleId);
                if (mRole != null)
                    throw new AlertException("已添加");

                var id = client.Insertable(new MemberRole
                {
                    ExpireDate = expDatetime ?? new DateTime(2099, 1, 1),
                    MemberId = memberId,
                    RoleId = roleId
                }).ExecuteReturnBigIdentity();
                return id > 0;
            }
        }

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<MemberLoginApo> LoginWithUsernamePassword(ServRequest<LoginWithUsernamePasswordDto> request)
        {
            static LoginWithUsernamePasswordQueryableItem GetMember(string username)
            {
                // ReSharper disable once ConvertToUsingDeclaration
                using (var client = DbFactory.GetClient())
                {
                    return client.Queryable<Member>()
                        .Select(it => new LoginWithUsernamePasswordQueryableItem
                        {
                            Id = it.Id,
                            UId = it.UId,
                            Username = it.Username,
                            Password = it.Password,
                            PasswordSalt = it.PasswordSalt,
                            IsDelete = it.IsDelete,
                            IsActive = it.IsActive,
                            CannotLoginUntilDate = it.CannotLoginUntilDate
                        })
                        .Single(t => t.Username.Equals(username));
                }
            }

            var member = GetMember(request.Data.Username);
            if (member == null)
            {
                return Error("用户不存在");
            }

            if (member.UId.IsNullOrEmpty())
            {
                return Error("用户UID为空");
            }

            if (member.IsDelete)
            {
                return Error("用户已删除");
            }

            if (!member.IsActive)
            {
                return Error("用户未激活");
            }

            if (member.CannotLoginUntilDate != null && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
            {
                return Error("用户未解封，请等待");
            }

            if (!PasswordsMatch(member.Password, member.PasswordSalt, request.Data.Password))
            {
                return Error("密码错误");
            }

            var accessToken = _jwtService.GenerateJwtToken(member.UId);
            return new MemberLoginApo
            {
                AccessToken = accessToken
            };
        }

        /// <summary>
        /// 三方平台登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<MemberLoginApo> ExternalAuth(ServRequest<ExternalAuthDto> request)
        {
            ExternalAuthentication externalAuthentication; 
            var memberUId = string.Empty;
            using (var db = DbFactory.GetClient())
            {
                externalAuthentication = db.Queryable<ExternalAuthentication>()
                    .Single(t => t.Openid.Equals(request.Data.OpenId) && t.Provider == request.Data.Provider);
            }

            if (externalAuthentication == null)
            {
                using (var db = DbFactory.GetClient())
                {
                    db.UseTran(tran =>
                    {
                        var identify = tran.Insertable(new Identify()).ExecuteReturnBigIdentity();
                        var registerMemberResult = _memberService.CreateMember(new CreateMemberDto
                        {
                            Avatar = request.Data.UserInfo.Avatar,
                            Username = $"{request.Data.Provider.ToString()}_{identify}"
                        }); 
                        tran.Insertable(new ExternalAuthentication()
                        {
                            Openid = request.Data.OpenId,
                            Provider = request.Data.Provider,
                            AccessToken = request.Data.AccessToken,
                            ExpireDt = request.Data.ExpireDt,
                            MemberId = registerMemberResult.Data.Id,
                            RefreshToken = request.Data.RefreshToken
                        }).ExecuteCommand();
                        memberUId = registerMemberResult.Data.UId;
                        return true;
                    });
                }
            }
            else
            {
                var memberId = externalAuthentication.MemberId;
                using (var db = DbFactory.GetClient())
                {
                    var member = db.Queryable<Member>()
                        .Select(it => new { it.Id, it.UId, it.Avatar })
                        .Single(it => it.Id == memberId);

                    externalAuthentication.AccessToken = request.Data.AccessToken;
                    externalAuthentication.ExpireDt = request.Data.ExpireDt;
                    externalAuthentication.RefreshToken = request.Data.RefreshToken;

                    db.UseTran(tran =>
                    {
                        tran.Updateable(externalAuthentication).ExecuteCommand();

                        if (member.Avatar.IsNullOrEmpty() && !(request?.Data?.UserInfo?.Avatar?.IsNullOrEmpty() ?? true))
                        {
                            tran.Updateable<Member>()
                                .SetColumns(it => new Member()
                                {
                                    Avatar = request.Data.UserInfo.Avatar
                                })
                                .Where(it => it.Id == memberId)
                                .ExecuteCommand();
                        }

                        return 0;
                    });

                    memberUId = member.UId;
                }
            }
            
            var accessToken = _jwtService.GenerateJwtToken(memberUId);
            return new MemberLoginApo
            {
                AccessToken = accessToken
            };
        }

        /// <summary>
        /// 设置cookie，用于web登录
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="MozException"></exception>
        public ServResult SetAuthCookie(ServRequest<SetAuthCookieDto> request)
        {
            if(request.Data.Token.IsNullOrEmpty())
                return Error("Token不能为空");
            
            var key = _mozOptions.Value.EncryptKey ?? "gvPXwK50tpE9b6P7";
            var encryptToken = _encryptionService.EncryptText(request.Data.Token, key);
            
            _httpContextAccessor.HttpContext?.Response?.Cookies?.Append("__moz__token",encryptToken,new CookieOptions()
            {
                Path = "/",
                HttpOnly = true
            });
            return Ok();
        }
        
        /// <summary>
        /// 退出web登录
        /// </summary>
        public ServResult RemoveAuthCookie()
        {
            _httpContextAccessor.HttpContext?.Response?.Cookies?.Delete("__moz__token");
            return Ok();
        }
        
    }
    
    internal class LoginWithUsernamePasswordQueryableItem
    {
        public long Id { get; set; }
        public string UId { get; set; }
        public string Username { get; set; }
        public string Password{ get; set; }
        public string PasswordSalt { get; set; }
        public bool IsDelete{ get; set; }
        public bool IsActive { get; set; }
        public DateTime? CannotLoginUntilDate{ get; set; }
    }
}