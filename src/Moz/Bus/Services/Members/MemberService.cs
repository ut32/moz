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

        public MemberService(
            IDistributedCache distributedCache,
            IEventPublisher eventPublisher,
            IEncryptionService encryptionService)
        {
            _distributedCache = distributedCache;
            _eventPublisher = eventPublisher;
            _encryptionService = encryptionService;
        }

        #region Utils
        
        /// <summary>
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private IEnumerable<Role> GetRolesByMemberId(long memberId)
        {
            var dt = DateTime.Now;
            using (var client = DbFactory.CreateClient())
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetMemberDetailApo> GetMemberDetail(GetMemberDetailDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var member = client.Queryable<Member>().InSingle(dto.Id);
                if (member == null)
                {
                    return Error("找不着信息");
                }

                var roles = client.Queryable<MemberRole>()
                    .Where(it => it.MemberId == dto.Id)
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
                using (var client = DbFactory.CreateClient())
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
            if (currentMember == null) 
                return null;

            currentMember.Roles = GetRolesByMemberId(currentMember.Id) ?? new List<Role>();
            currentMember.Permissions = new List<Permission>();
            return currentMember;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ResetPassword(ResetPasswordDto dto)
        {
            using (var db = DbFactory.CreateClient())
            {
                var members = db.Queryable<Member>()
                    .Select(it => new Member {Id = it.Id, Password = it.Password, PasswordSalt = it.PasswordSalt})
                    .Where(it => dto.MemberIds.Contains(it.Id))
                    .ToList();

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService
                    .CreatePasswordHash(dto.NewPassword, saltKey, "SHA512");

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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<CreateMemberApo> CreateMember(CreateMemberDto dto)
        {
            var member = new Member
            {
                UId = Guid.NewGuid().ToString("N"),
                Address = null,
                Avatar = dto.Avatar,
                Nickname = dto.Nickname,
                Birthday = null,
                CannotLoginUntilDate = null,
                Email = null,
                FailedLoginAttempts = 0,
                Gender = dto.Gender,
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
                Username = dto.Username
            };
            using (var db = DbFactory.CreateClient())
            {
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Username.Equals(dto.Username))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此用户名 {dto.Username} 已存在");
                    }

                    member.Username = dto.Username;
                }
                
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(dto.Password, saltKey, "SHA512");

                    member.Password = newPassword;
                    member.PasswordSalt = saltKey;
                }

                if (!dto.Email.IsNullOrEmpty())
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Email.Equals(dto.Email))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此邮箱 {dto.Email} 已存在");
                    }

                    member.Email = dto.Email;
                }

                if (!dto.Mobile.IsNullOrEmpty())
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Mobile.Equals(dto.Mobile))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此手机 {dto.Mobile} 已存在");
                    }

                    member.Mobile = dto.Mobile;
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateMember(UpdateMemberDto dto)
        {
            using (var db = DbFactory.CreateClient())
            {
                var member = db.Queryable<Member>().InSingle(dto.Id);
                if (member == null)
                {
                    return Error("找不到该用户");
                }

                if (!member.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Username.Equals(dto.Username))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此用户名 {dto.Username} 已存在");
                    }

                    member.Username = dto.Username;
                }

                if (!dto.Password.Equals("******"))
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(dto.Password, saltKey, "SHA512");

                    member.Password = newPassword;
                    member.PasswordSalt = saltKey;
                }

                if (!dto.Email.IsNullOrEmpty() &&
                    !dto.Email.Equals(member.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Email.Equals(dto.Email))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此邮箱 {dto.Email} 已存在");
                    }
                    member.Email = dto.Email;
                }

                if (!dto.Mobile.IsNullOrEmpty() &&
                    !dto.Mobile.Equals(member.Mobile, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Mobile.Equals(dto.Mobile))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此手机 {dto.Mobile} 已存在");
                    }

                    member.Mobile = dto.Mobile;
                }

                member.Nickname = dto.Nickname;
                member.Avatar = dto.Avatar;
                member.Gender = dto.Gender;
                member.Birthday = dto.BirthDay;
                
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

                    if (dto.Roles != null && dto.Roles.Length > 0)
                    {
                        var newRoles = dto.Roles.Select(it => new MemberRole
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ChangePassword(ChangePasswordDto dto)
        {
            using (var db = DbFactory.CreateClient())
            {
                var member = db.Queryable<Member>().InSingle(dto.MemberId);
                if (member == null)
                {
                    return Error("找不到该用户");
                }
                
                var inputEncryptOldPassword = _encryptionService.CreatePasswordHash(dto.OldPassword, member.PasswordSalt, "SHA512");
                if (!inputEncryptOldPassword.Equals(member.Password, StringComparison.OrdinalIgnoreCase))
                {
                    return Error("原密码不正确");
                }

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService.CreatePasswordHash(dto.NewPassword, saltKey, "SHA512");

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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateAvatar(UpdateAvatarDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var member = client.Queryable<Member>()
                    .Select(it => new {it.Id, it.UId})
                    .First(it => it.Id == dto.MemberId);
                if (member == null)
                    return Error("没找到对象");
                
                client.Updateable<Member>()
                    .SetColumns(it => new Member() { Avatar = dto.Avatar })
                    .Where(it=>it.Id==dto.MemberId)
                    .ExecuteCommand();
                
                _distributedCache.Remove($"CACHE_MEMBER_{member.UId}");
            }
            
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateUsername(UpdateUsernameDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var member = client.Queryable<Member>()
                    .Select(it => new {it.Id, it.UId, it.Username})
                    .First(it => it.Id == dto.MemberId);
                if (member == null)
                    return Error("没找到对象");

                if (member.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase))
                    return Error("用户名未变更");

                var isExist = client.Queryable<Member>()
                    .Select(it => new { it.Id,it.Username })
                    .First(it => it.Username.Equals(dto.Username))!=null;
                if (isExist)
                    return Error($"此用户名 {dto.Username} 已存在");

                client.Updateable<Member>()
                    .SetColumns(it => new Member() { Username = dto.Username })
                    .Where(it => it.Id == dto.MemberId)
                    .ExecuteCommand();

                _distributedCache.Remove($"CACHE_MEMBER_{member.UId}");
            }

            return Ok();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryMemberItem>> PagedQueryMembers(PagedQueryMemberDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<Member>()
                    .WhereIF(!dto.Keyword.IsNullOrEmpty(), t => t.Username.Contains(dto.Keyword))
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