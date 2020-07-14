using System;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Common;
using Moz.Bus.Models.Members;
using Moz.DataBase;
using Moz.Events;

namespace Moz.Bus.Services.Members
{
    public class RegistrationService :BaseService, IRegistrationService
    {
        private readonly IEventPublisher _eventPublisher;

        public RegistrationService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 三方平台注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<RegistrationResult> Register(ExternalRegistrationDto dto)
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
            
            var result = new RegistrationResult
            {
                MemberId = memberId,
                MemberUId = memberUId
            };
            
            _eventPublisher.Publish(result);
            
            return result;
        }
    }
}