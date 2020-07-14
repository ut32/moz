using System;
using System.Linq;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Common;
using Moz.Bus.Models.Members;
using Moz.DataBase;
using Moz.Events;
using Moz.Service.Security;

namespace Moz.Bus.Services.Members
{
    public class RegistrationService :BaseService, IRegistrationService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEncryptionService _encryptionService;

        public RegistrationService(IEventPublisher eventPublisher, IEncryptionService encryptionService)
        {
            _eventPublisher = eventPublisher;
            _encryptionService = encryptionService;
        }

        /// <summary>
        /// 三方平台注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<RegistrationInfo> Register(ExternalRegistrationDto dto)
        {
            var memberUId = string.Empty;
            var memberId = 0L;
            using (var db = DbFactory.CreateClient())
            {
                db.UseTran(tran =>
                {
                    var identify = tran.Insertable(new Identify()).ExecuteReturnBigIdentity();
                    memberUId = Guid.NewGuid().ToString("N");
                    var member = new Member
                    {
                        UId = memberUId,
                        Address = null,
                        Avatar = dto.UserInfo?.Avatar,
                        Nickname = dto.UserInfo?.Nickname,
                        Birthday = null,
                        CannotLoginUntilDate = null,
                        Email = null,
                        FailedLoginAttempts = 0,
                        Gender = GenderEnum.Man,
                        Geohash = null,
                        IsActive = true,
                        IsDelete = false,
                        IsEmailValid = false,
                        IsMobileValid = false,
                        LastActiveDatetime = DateTime.Now,
                        LastLoginDatetime = DateTime.Now,
                        LastLoginIp = null,
                        Lat = null,
                        Lng = null,
                        LoginCount = 0,
                        Mobile = null,
                        OnlineTimeCount = 0,
                        Password = $"pwd_{identify}",
                        PasswordSalt = "---",
                        RegionCode = null,
                        RegisterDatetime = DateTime.Now,
                        RegisterIp = null,
                        Username = $"{dto.Provider.ToString()}_{identify}"
                    };
                    memberId = tran.Insertable(member).ExecuteReturnBigIdentity();
                    tran.Insertable(new MemberRole
                    {
                        ExpireDate = null,
                        MemberId = memberId,
                        RoleId = 3
                    }).ExecuteCommand();
                    tran.Insertable(new ExternalAuthentication()
                    {
                        Openid = dto.OpenId,
                        Provider = dto.Provider,
                        AccessToken = dto.AccessToken,
                        ExpireDt = dto.ExpireDt,
                        MemberId = memberId,
                        RefreshToken = dto.RefreshToken
                    }).ExecuteCommand();
                });
            }
            
            var result = new RegistrationInfo
            {
                MemberId = memberId,
                MemberUId = memberUId
            };
            
            _eventPublisher.Publish(result);
            
            return result;
        }

        /// <summary>
        /// 用户名密码注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<RegistrationInfo> Register(UsernameRegistrationDto dto)
        {
            var memberUId = string.Empty;
            var memberId = 0L;
            using (var db = DbFactory.CreateClient())
            {
                var isExist = db.Queryable<Member>()
                    .Where(it => it.Username.Equals(dto.Username))
                    .Select(it => new {it.Id})
                    .ToList()
                    .Any();
                if (isExist)
                {
                    return Error($"用户名 {dto.Username} 已存在");
                }

                db.UseTran(tran =>
                {
                    memberUId = Guid.NewGuid().ToString("N");
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var hashedPassword = _encryptionService.CreatePasswordHash(dto.Password, saltKey, "SHA512");
                    var member = new Member
                    {
                        UId = memberUId,
                        Address = null,
                        Avatar = null,
                        Nickname = null,
                        Birthday = null,
                        CannotLoginUntilDate = null,
                        Email = null,
                        FailedLoginAttempts = 0,
                        Gender = GenderEnum.Man,
                        Geohash = null,
                        IsActive = true,
                        IsDelete = false,
                        IsEmailValid = false,
                        IsMobileValid = false,
                        LastActiveDatetime = DateTime.Now,
                        LastLoginDatetime = DateTime.Now,
                        LastLoginIp = null,
                        Lat = null,
                        Lng = null,
                        LoginCount = 0,
                        Mobile = null,
                        OnlineTimeCount = 0,
                        Password = hashedPassword,
                        PasswordSalt = saltKey,
                        RegionCode = null,
                        RegisterDatetime = DateTime.Now,
                        RegisterIp = null,
                        Username = dto.Username
                    };
                    memberId = tran.Insertable(member).ExecuteReturnBigIdentity();
                    tran.Insertable(new MemberRole
                    {
                        ExpireDate = null,
                        MemberId = memberId,
                        RoleId = 3
                    }).ExecuteCommand();
                });
            }

            var result = new RegistrationInfo
            {
                MemberId = memberId,
                MemberUId = memberUId
            };

            _eventPublisher.Publish(result);

            return result;
        }
    }
}