using System;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Request.Members;
using Moz.Bus.Dtos.Result.Members;
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
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<RegistrationResult> Register(ServRequest<ExternalRegistrationRequest> request)
        {
            var memberUId = string.Empty;
            var memberId = 0L;
            using (var db = DbFactory.GetClient())
            {
                db.UseTran(tran =>
                {
                    var identify = tran.Insertable(new Identify()).ExecuteReturnBigIdentity();
                    memberUId = Guid.NewGuid().ToString("N");
                    var member = new Member
                    {
                        UId = memberUId,
                        Address = null,
                        Avatar = request.Data.UserInfo?.Avatar,
                        Nickname = request.Data.UserInfo?.Nickname,
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
                        Username = $"{request.Data.Provider.ToString()}_{identify}"
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
                        Openid = request.Data.OpenId,
                        Provider = request.Data.Provider,
                        AccessToken = request.Data.AccessToken,
                        ExpireDt = request.Data.ExpireDt,
                        MemberId = memberId,
                        RefreshToken = request.Data.RefreshToken
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