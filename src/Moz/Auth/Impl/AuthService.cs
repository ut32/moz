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
using Moz.Bus.Dtos.Auth;
using Moz.Bus.Models.Common;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Members;
using Moz.CMS.Models.Members;
using Moz.Core.Options;
using Moz.DataBase;
using Moz.Exceptions;
using Moz.Service.Security;

namespace Moz.Auth.Impl
{
    internal class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemberService _memberService;
        private readonly IEncryptionService _encryptionService;
        private readonly IOptions<MozOptions> _mozOptions;
        private readonly IDistributedCache _distributedCache;

        public AuthService(IHttpContextAccessor httpContextAccessor,
            IMemberService memberService,
            IEncryptionService encryptionService,
            IOptions<MozOptions> mozOptions,
            IDistributedCache distributedCache
            )
        { 
            _httpContextAccessor = httpContextAccessor;
            _memberService = memberService;
            _encryptionService = encryptionService;
            _mozOptions = mozOptions;
            _distributedCache = distributedCache;
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
        private string GenerateJwtToken(long memberId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,memberId.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,DateTime.Now.AddDays(90).ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mozOptions.Value.EncryptKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                "https://136cc.com",
                "moz_application",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
            var uid = GetAuthenticatedUserId();
            if (uid == 0)
            {
                return null;
            }
            
            var member = _memberService.GetSimpleMemberById(uid);
            if (member == null) return null;

            if (!member.IsActive) return null;
            if (member.IsDelete) return null;
            if (member.CannotLoginUntilDate.HasValue && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
                return null;

            return member;
        }

        public long GetAuthenticatedUserId()
        {
            var result = _httpContextAccessor
                .HttpContext
                .AuthenticateAsync(MozAuthAttribute.MozAuthorizeSchemes)
                .GetAwaiter()
                .GetResult();
            if (!result?.Principal?.Identity?.IsAuthenticated??false) return 0;
            
            var claim = result?.Principal?.Claims?.FirstOrDefault(o => o.Type == "jti");
            if (claim == null || claim.Value.IsNullOrEmpty()) return 0;

            long.TryParse(claim.Value,out var uid);

            return uid;
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
        /// 用户密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LoginWithPasswordResponse LoginWithPassword(LoginWithPasswordRequest request)
        {
            var response = new LoginWithPasswordResponse();
            try
            {
                Member GetMember(string username)
                {
                    using var client = DbFactory.GetClient();
                    return client.Queryable<Member>()
                        .Select(it=>new Member
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

                var member = GetMember(request.Username);
                if (member == null)
                {
                    response.Code = 1001;
                    response.Message = "用户不存在";
                    return response;
                }

                if (member.IsDelete)
                {
                    response.Code = 1002;
                    response.Message = "用户已删除";
                    return response;
                }
                
                if (!member.IsActive)
                {
                    response.Code = 1003;
                    response.Message = "用户被屏蔽";
                    return response;
                }
                
                if (member.CannotLoginUntilDate.HasValue && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
                {
                    response.Code = 1005;
                    response.Message = "用户未解封，请等待";
                    return response;
                }
                
                if (!PasswordsMatch(member.Password, member.PasswordSalt, request.Password))
                {
                    response.Code = 1006;
                    response.Message = "密码错误";
                    return response;
                }


                return new LoginWithPasswordResponse
                {
                    MemberId = member.Id,
                    AccessToken = GenerateJwtToken(member.Id)
                };
            }
            catch (Exception e)
            {
                response.Code = 999;
                response.Message = e.Message;
            }

            return response;
        }

        /// <summary>
        /// 三方登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExternalAuthResponse ExternalAuth(ExternalAuthRequest request)
        {
            ExternalAuthentication externalAuthentication;
            var memberId = 0L;
            using (var db = DbFactory.GetClient())
            {
                externalAuthentication = db.Queryable<ExternalAuthentication>().Single(t => t.Openid.Equals(request.OpenId) && t.Provider == request.Provider);
            }

            if (externalAuthentication == null)
            {
                using var db = DbFactory.GetClient();
                memberId = db.UseTran(tran =>
                {
                    var identify = tran.Insertable(new Identify()).ExecuteReturnBigIdentity();
                    return tran.Insertable(new Member
                    {
                        UId = Guid.NewGuid().ToString("N"),
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
                        PasswordSalt =  _encryptionService.CreateSaltKey(6),
                        RegionCode = null,
                        RegisterDatetime = DateTime.UtcNow,
                        RegisterIp = null,
                        Username = $"{request?.UserInfo?.Nickname}_{identify}"
                    }).ExecuteReturnBigIdentity();
                });
            }
            else
            {
                memberId = externalAuthentication.MemberId;
                using var db = DbFactory.GetClient();
                var id = memberId;
                var member = db.Queryable<Member>()
                    .Select(it => new {it.Id, it.Avatar})
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
            }
            
            var token = GenerateJwtToken(memberId);
            return new ExternalAuthResponse
            {
                MemberId = memberId,
                Token = token
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
}