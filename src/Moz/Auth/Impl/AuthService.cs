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
using Moz.Bus.Models.Common;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Members;
using Moz.CMS.Models.Members;
using Moz.Core.Options;
using Moz.DataBase;
using Moz.Exceptions;
using Moz.Service.Security;
using Moz.WebApi;

namespace Moz.Auth.Impl
{
    internal class AuthService : IAuthService
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

        private string GenerateRefreshToken()
        {
            var unixTime = DateTime.Now.AddHours(2).ToUnixTime();
            var guid = Guid.NewGuid().ToString("N");
            var finalText = $"{unixTime}|{guid}";
            return _encryptionService.EncryptText(finalText);
        }

        #endregion

        public SimpleMember GetAuthenticatedMember()
        {
            var uid = GetAuthenticatedUId();
            if (uid.IsNullOrEmpty())
            {
                return null;
            }
            
            var member = _memberService.GetSimpleMemberByUId(uid);
            if (member == null) return null;

            if (!member.IsActive) return null;
            if (member.IsDelete) return null;
            if (member.CannotLoginUntilDate.HasValue && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
                return null;

            return member;
        }

        public string GetAuthenticatedUId()
        {
            var result = _httpContextAccessor
                .HttpContext
                .AuthenticateAsync(MozAuthAttribute.MozAuthorizeSchemes)
                .GetAwaiter()
                .GetResult();
            if (!result?.Principal?.Identity?.IsAuthenticated??false) return null;
            
            var claim = result?.Principal?.Claims?.FirstOrDefault(o => o.Type == "jti");
            if (claim == null || claim.Value.IsNullOrEmpty()) return null;

            return claim.Value;
        }

        public bool AddRoleToMemberId(long memberId, long roleId, DateTime? expDatetime=null) 
        { 
            using (var client = DbFactory.GetClient())
            { 
                var mRole = client.Queryable<MemberRole>()
                    .Single(t => t.MemberId == memberId && t.RoleId == roleId);
                if (mRole != null)
                    throw new MozException("已添加");

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
        public MemberLoginResult LoginWithUsernamePassword(MemberLoginRequest request)
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
            
            var memberLoginResult = new MemberLoginResult();
            if (request.Username.IsNullOrEmpty()) 
            {
                memberLoginResult.AddError("用户名不能为空");
                return memberLoginResult;
            }
            
            if (request.Password.IsNullOrEmpty())
            {
                memberLoginResult.AddError("密码不能为空");
                return memberLoginResult;
            }

            var member = GetMember(request.Username);
            if (member == null)
            {
                memberLoginResult.AddError("用户不存在");
                return memberLoginResult;
            }

            if (member.UId.IsNullOrEmpty())
            {
                memberLoginResult.AddError("用户UID为空");
                return memberLoginResult;
            }

            if (member.IsDelete)
            {
                memberLoginResult.AddError("用户已删除");
                return memberLoginResult;
            }

            if (!member.IsActive)
            {
                memberLoginResult.AddError("用户未激活");
                return memberLoginResult;
            }

            if (member.CannotLoginUntilDate != null && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
            {
                memberLoginResult.AddError("用户未解封，请等待");
                return memberLoginResult;
            }

            if (!PasswordsMatch(member.Password, member.PasswordSalt, request.Password))
            {
                memberLoginResult.AddError("密码错误");
                return memberLoginResult;
            }

            var accessToken = _jwtService.GenerateJwtToken(member.UId);
            return new MemberLoginResult
            {
                AccessToken = accessToken
            };
        }

        /// <summary>
        /// 三方平台登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MemberLoginResult ExternalAuth(ExternalAuthRequest request)
        {
            ExternalAuthentication externalAuthentication; 
            var memberUId = string.Empty;
            var memberId = 0L;
            using (var db = DbFactory.GetClient())
            {
                externalAuthentication = db.Queryable<ExternalAuthentication>()
                    .Single(t => t.Openid.Equals(request.OpenId) && t.Provider == request.Provider);
            }

            if (externalAuthentication == null)
            {
                using (var db = DbFactory.GetClient())
                {
                    memberId = db.UseTran(tran =>
                    {
                        var identify = tran.Insertable(new Identify()).ExecuteReturnBigIdentity();
                        memberUId = Guid.NewGuid().ToString("N");
                        return tran.Insertable(new Member
                        {
                            UId = memberUId,
                            Address = null,
                            Avatar = request?.UserInfo?.Avatar,
                            Nickname = request?.UserInfo?.Nickname,
                            Birthday = null,
                            CannotLoginUntilDate = null,
                            Email = null,
                            FailedLoginAttempts = 0,
                            Gender = null,
                            Geohash = null,
                            IsActive = true,
                            IsDelete = false,
                            IsEmailValid = false,
                            IsMobileValid = false,
                            LastActiveDatetime = DateTime.UtcNow,
                            LastLoginDatetime = DateTime.UtcNow,
                            LastLoginIp = null,
                            Lat = null,
                            Lng = null,
                            LoginCount = 0,
                            Mobile = null,
                            OnlineTimeCount = 0,
                            Password = _encryptionService.CreateSaltKey(10),
                            PasswordSalt = _encryptionService.CreateSaltKey(6),
                            RegionCode = null,
                            RegisterDatetime = DateTime.UtcNow,
                            RegisterIp = null,
                            Username = $"{request?.UserInfo?.Nickname}_{identify}"
                        }).ExecuteReturnBigIdentity();
                    });
                }
            }
            else
            {
                memberId = externalAuthentication.MemberId;
                using (var db = DbFactory.GetClient())
                {
                    var id = memberId;
                    var member = db.Queryable<Member>()
                        .Select(it => new {it.Id, it.UId, it.Avatar })
                        .Single(it => it.Id == id);

                    externalAuthentication.AccessToken = request.AccessToken;
                    externalAuthentication.ExpireDt = request.ExpireDt;
                    externalAuthentication.RefreshToken = request.RefreshToken;

                    db.UseTran(tran =>
                    {
                        tran.Updateable(externalAuthentication).ExecuteCommand();

                        if (member.Avatar.IsNullOrEmpty() && !(request?.UserInfo?.Avatar?.IsNullOrEmpty() ?? true))
                        {
                            tran.Updateable<Member>()
                                .UpdateColumns(it => new Member()
                                {
                                    Avatar = request.UserInfo.Avatar
                                })
                                .Where(it => it.Id == id)
                                .ExecuteCommand();
                        }

                        return 0;
                    });

                    memberUId = member.UId;
                }
            }
            
            var accessToken = _jwtService.GenerateJwtToken(memberUId);
            return new MemberLoginResult
            {
                AccessToken = accessToken
            };
        }
        
        /// <summary>
        /// 设置cookie，用于web登录
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="MozException"></exception>
        public void SetAuthCookie(string token)
        {
            if(token.IsNullOrEmpty())
                throw new MozException("token required");
            
            var key = _mozOptions.Value.EncryptKey ?? "gvPXwK50tpE9b6P7";
            var encryptToken = _encryptionService.EncryptText(token, key);
            
            _httpContextAccessor.HttpContext?.Response?.Cookies?.Append("__moz__token",encryptToken,new CookieOptions()
            {
                Path = "/",
                HttpOnly = true
            });
        }

        
        /// <summary>
        /// 退出web登录
        /// </summary>
        public void RemoveAuthCookie()
        {
            _httpContextAccessor.HttpContext?.Response?.Cookies?.Delete("__moz__token");
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