using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.AdminMenus;
using Moz.Bus.Models.Members;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;
using Moz.Service.Security;
using Moz.Utils;
using Moz.Utils.Impl;
using SqlSugar;

namespace Moz.Bus.Services.Members
{
    public class MemberService :BaseService, IMemberService
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly MemberSettings _memberSettings;
        private IMemberService _memberServiceImplementation;

        public MemberService(
            IDistributedCache distributedCache,
            IEventPublisher eventPublisher,
            IEncryptionService encryptionService,
            MemberSettings memberSettings)
        {
            _distributedCache = distributedCache;
            _eventPublisher = eventPublisher;
            _encryptionService = encryptionService;
            _memberSettings = memberSettings;
        }

        #region Utils
        
        /// <summary>
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private IEnumerable<Role> GetRolesByMemberId(long memberId)
        {
            var dt = DateTime.Now;
            using (var client = DbFactory.GetClient())
            {
                return client.Queryable<MemberRole, Role>((mr, r) => new object[]
                    {
                        JoinType.Left, mr.RoleId == r.Id
                    })
                    .Where((mr, r) => mr.MemberId == memberId 
                                      && (mr.ExpireDate == null || mr.ExpireDate != null && mr.ExpireDate > dt)
                                      && r.IsActive)
                    .Select((mr, r) => new 
                    {
                        r.Id,
                        r.Code,
                        r.IsActive,
                        r.Name,
                        r.IsAdmin,
                        mr.ExpireDate
                    })
                    .ToList()
                    .Select(it=> new Role()
                    {
                        Code = it.Code,
                        Id = it.Id,
                        Name = it.Name,
                        IsActive = it.IsActive,
                        IsAdmin = it.IsAdmin
                    });
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetMemberDetailApo> GetMemberDetail(ServRequest<GetMemberDetailDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var member = client.Queryable<Member>().InSingle(request.Data.Id);
                if (member == null)
                {
                    return Error("找不着信息");
                }

                var roles = client.Queryable<MemberRole>()
                    .Where(it => it.MemberId == request.Data.Id)
                    .Select(it => new {it.RoleId})
                    .ToList()
                    .Select(it => it.RoleId).ToArray();

                var resp = new GetMemberDetailApo
                {
                    Id = member.Id,
                    Username = member.Username,
                    Email = member.Email,
                    Mobile = member.Mobile,
                    Avatar = member.Avatar,
                    Gender = member.Gender,
                    Birthday = member.Birthday,
                    RegisterIp = member.RegisterIp,
                    RegisterDatetime = member.RegisterDatetime,
                    LoginCount = member.LoginCount,
                    LastLoginIp = member.LastLoginIp,
                    LastLoginDatetime = member.LastLoginDatetime,
                    CannotLoginUntilDate = member.CannotLoginUntilDate,
                    LastActiveDatetime = member.LastActiveDatetime,
                    FailedLoginAttempts = member.FailedLoginAttempts,
                    OnlineTimeCount = member.OnlineTimeCount,
                    Address = member.Address,
                    RegionCode = member.RegionCode,
                    Lng = member.Lng,
                    Lat = member.Lat,
                    Geohash = member.Geohash,
                    IsActive = member.IsActive,
                    IsDelete = member.IsDelete,
                    IsEmailValid = member.IsEmailValid,
                    IsMobileValid = member.IsMobileValid,
                    Nickname = member.Nickname,
                    Roles = roles
                };
                return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public SimpleMember GetSimpleMemberByUId(string uid)
        {
            SimpleMember GetSimpleMember(string newUid) 
            {
                using (var client = DbFactory.GetClient())
                {
                    var member = client.Queryable<Member>().Select(t => new
                    {
                        t.Id,
                        t.UId,
                        t.Email,
                        t.Username,
                        t.Nickname,
                        t.Mobile,
                        t.IsActive,
                        t.IsDelete,
                        t.Avatar,
                        t.CannotLoginUntilDate
                    }).Single(t => t.UId == newUid);

                    if (member == null)
                        return null;

                    return new SimpleMember
                    {
                        Id = member.Id,
                        UId = member.UId,
                        Email = member.Email,
                        Username = member.Username,
                        Nickname = member.Nickname,
                        Mobile = member.Mobile,
                        Avatar = member.Avatar,
                        IsActive = member.IsActive,
                        IsDelete = member.IsDelete,
                        CannotLoginUntilDate = member.CannotLoginUntilDate
                    };
                }
            }
            
            var currentMember =  GetSimpleMember(uid);
            if (currentMember == null) return null;

            currentMember.Roles = GetRolesByMemberId(currentMember.Id) ?? new List<Role>();
            currentMember.Permissions = new List<Permission>();
            return currentMember;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult ResetPassword(ServRequest<ResetPasswordDto> request)
        {
            using (var db = DbFactory.GetClient())
            {
                var members = db.Queryable<Member>()
                    .Select(it => new Member {Id = it.Id, Password = it.Password, PasswordSalt = it.PasswordSalt})
                    .Where(it => request.Data.MemberIds.Contains(it.Id))
                    .ToList();

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService
                    .CreatePasswordHash(request.Data.NewPassword, saltKey, _memberSettings.HashedPasswordFormat);

                members.ForEach(it =>
                {
                    it.Password = newPassword;
                    it.PasswordSalt = saltKey;
                });
                db.Updateable(members)
                    .UpdateColumns(it => new
                    {
                        it.Password,
                        it.PasswordSalt
                    })
                    .ExecuteCommand();

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<CreateMemberApo> CreateMember(ServRequest<CreateMemberDto> request)
        {
            var member = new Member
            {
                UId = Guid.NewGuid().ToString("N"),
                Address = null,
                Avatar = request.Data.Avatar,
                Nickname = request.Data.Nickname,
                Birthday = null,
                CannotLoginUntilDate = null,
                Email = null,
                FailedLoginAttempts = 0,
                Gender = request.Data.Gender,
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
                Password = null,
                PasswordSalt = null,
                RegionCode = null,
                RegisterDatetime = DateTime.Now,
                RegisterIp = null,
                Username = request.Data.Username
            };
            using (var db = DbFactory.GetClient())
            {
                
                
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Username.Equals(request.Data.Username))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此用户名 {request.Data.Username} 已存在");
                    }

                    member.Username = request.Data.Username;
                }
                
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(request.Data.Password, saltKey, _memberSettings.HashedPasswordFormat);

                    member.Password = newPassword;
                    member.PasswordSalt = saltKey;
                }

                if (!request.Data.Email.IsNullOrEmpty())
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Email.Equals(request.Data.Email))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此邮箱 {request.Data.Email} 已存在");
                    }

                    member.Email = request.Data.Email;
                }

                if (!request.Data.Mobile.IsNullOrEmpty())
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Mobile.Equals(request.Data.Mobile))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此手机 {request.Data.Mobile} 已存在");
                    }

                    member.Mobile = request.Data.Mobile;
                }

                db.UseTran(tran =>
                {
                    member.Id = tran.Insertable(member).ExecuteReturnBigIdentity();
                    tran.Insertable(new MemberRole
                    {
                        ExpireDate = null,
                        MemberId = member.Id,
                        RoleId = 3
                    }).ExecuteCommand();
                    return 0;
                });

            }

            return new CreateMemberApo
            {
                Id = member.Id,
                UId = member.UId,
                Username = member.Username
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ServResult UpdateMember(ServRequest<UpdateMemberDto> request)
        {
            using (var db = DbFactory.GetClient())
            {
                var member = db.Queryable<Member>().InSingle(request.Data.Id);
                if (member == null)
                {
                    return Error("找不到该用户");
                }

                if (!member.Username.Equals(request.Data.Username, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Username.Equals(request.Data.Username))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此用户名 {request.Data.Username} 已存在");
                    }

                    member.Username = request.Data.Username;
                }

                if (!request.Data.Password.Equals("******"))
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(request.Data.Password, saltKey, _memberSettings.HashedPasswordFormat);

                    member.Password = newPassword;
                    member.PasswordSalt = saltKey;
                }

                if (!request.Data.Email.IsNullOrEmpty() &&
                    !request.Data.Email.Equals(member.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Email.Equals(request.Data.Email))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此邮箱 {request.Data.Email} 已存在");
                    }
                    member.Email = request.Data.Email;
                }

                if (!request.Data.Mobile.IsNullOrEmpty() &&
                    !request.Data.Mobile.Equals(member.Mobile, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Mobile.Equals(request.Data.Mobile))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此手机 {request.Data.Mobile} 已存在");
                    }

                    member.Mobile = request.Data.Mobile;
                }

                member.Nickname = request.Data.Nickname;
                member.Avatar = request.Data.Avatar;
                member.Gender = request.Data.Gender;
                member.Birthday = request.Data.BirthDay;
                
                db.UseTran(tran =>
                {
                    tran.Updateable(member).UpdateColumns(it => new
                    {
                        it.Username,
                        it.Password,
                        it.PasswordSalt,
                        it.Email,
                        it.Mobile,
                        it.Nickname,
                        it.Avatar,
                        it.Gender,
                        it.Birthday
                    }).ExecuteCommand();

                    tran.Deleteable<MemberRole>().Where(it => it.MemberId == member.Id).ExecuteCommand();

                    if (request.Data.Roles != null && request.Data.Roles.Length > 0)
                    {
                        var newRoles = request.Data.Roles.Select(it => new MemberRole
                        {
                            ExpireDate = null,
                            MemberId = member.Id,
                            RoleId = it
                        }).ToList();
                        tran.Insertable(newRoles).ExecuteCommand();
                    }

                    return 0;
                });
                
                _distributedCache.Remove($"CACHE_MEMBER_{member.UId}");

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ServResult ChangePassword(ServRequest<ChangePasswordDto> request)
        {
            using (var db = DbFactory.GetClient())
            {
                var member = db.Queryable<Member>().InSingle(request.Data.MemberId);
                if (member == null)
                {
                    return Error("找不到该用户");
                }
                
                var inputEncryptOldPassword = _encryptionService.CreatePasswordHash(request.Data.OldPassword, member.PasswordSalt, _memberSettings.HashedPasswordFormat);
                if (!inputEncryptOldPassword.Equals(member.Password, StringComparison.OrdinalIgnoreCase))
                {
                    return Error("原密码不正确");
                }

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService.CreatePasswordHash(request.Data.NewPassword, saltKey, _memberSettings.HashedPasswordFormat);

                member.Password = newPassword;
                member.PasswordSalt = saltKey;

                db.Updateable(member).UpdateColumns(it => new
                {
                    it.Password,
                    it.PasswordSalt
                }).ExecuteCommand();
            }

            return Ok();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<PagedList<QueryMemberItem>> PagedQueryMembers(ServRequest<PagedQueryMemberDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Member>()
                    .WhereIF(!request.Data.Keyword.IsNullOrEmpty(), t => t.Username.Contains(request.Data.Keyword))
                    .Select(t => new QueryMemberItem()
                    {
                        Id = t.Id,
                        Username = t.Username,
                        Password = t.Password,
                        PasswordSalt = t.PasswordSalt,
                        Email = t.Email,
                        Mobile = t.Mobile,
                        Avatar = t.Avatar,
                        Gender = t.Gender,
                        Birthday = t.Birthday,
                        RegisterIp = t.RegisterIp,
                        RegisterDatetime = t.RegisterDatetime,
                        LoginCount = t.LoginCount,
                        LastLoginIp = t.LastLoginIp,
                        LastLoginDatetime = t.LastLoginDatetime,
                        CannotLoginUntilDate = t.CannotLoginUntilDate,
                        LastActiveDatetime = t.LastActiveDatetime,
                        FailedLoginAttempts = t.FailedLoginAttempts,
                        OnlineTimeCount = t.OnlineTimeCount,
                        Address = t.Address,
                        RegionCode = t.RegionCode,
                        Lng = t.Lng,
                        Lat = t.Lat,
                        Geohash = t.Geohash,
                        IsActive = t.IsActive,
                        IsDelete = t.IsDelete,
                        IsEmailValid = t.IsEmailValid,
                        IsMobileValid = t.IsMobileValid,
                        Nickname = t.Nickname,
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize, ref total);

                var memberIds = list.Select(it => it.Id).ToArray();
                var roles = client.Queryable<MemberRole, Role>((mr, r) => new object[]
                    {
                        JoinType.Left, mr.RoleId == r.Id
                    })
                    .Where((mr, r) => memberIds.Contains(mr.MemberId))
                    .Select((mr, r) => new
                    {
                        mr.MemberId,
                        r.Name
                    })
                    .ToList();
                list.ForEach(it => { it.Roles = roles.Where(x => x.MemberId == it.Id).Select(x => x.Name).ToArray(); });

                return new PagedList<QueryMemberItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        #endregion
    }
}