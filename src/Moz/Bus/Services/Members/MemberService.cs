using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Biz.Dtos.Members;
using Moz.Biz.Models.AdminMenus;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Dtos.Members.Roles;
using Moz.Bus.Models.Members;
using Moz.CMS.Model.AdminMenus;
using Moz.CMS.Models.Members;
using Moz.Core.Service.Members;
using Moz.DataBase;
using Moz.Domain.Dtos.Members.Permissions;
using Moz.Domain.Dtos.Members.Roles;
using Moz.Events;
using Moz.Exceptions;
using Moz.Service.Security;
using Moz.Utils;
using Moz.Utils.Impl;
using SqlSugar;

namespace Moz.Bus.Services.Members
{
    public class MemberService : IMemberService
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly MemberSettings _memberSettings;

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

        #region 用户管理
        
        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetMemberDetailResponse GetMemberDetail(GetMemberDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                 var member = client.Queryable<Member>().InSingle(request.Id);
                 if(member == null)
                 {
                    return null;
                 }

                 var roles = client.Queryable<MemberRole>()
                     .Where(it=>it.MemberId == request.Id)
                     .Select(it=>new { it.RoleId })
                     .ToList()
                     .Select(it=>it.RoleId).
                     ToArray();

                 var resp = new GetMemberDetailResponse
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
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryMemberResponse PagedQueryMembers(PagedQueryMemberRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Member>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Username.Contains(request.Keyword))
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

                return new PagedQueryMemberResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SimpleMember GetSimpleMemberById(long id)
        {
            SimpleMember GetSimpleMember(long newid)
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
                    }).Single(t => t.Id == newid);

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

            var t1 = Task.Run(() => GetSimpleMember(id));
            var t2 = Task.Run(() => GetAvailablePermissionsByMemeberId(id));
            var t3 = Task.Run(() => GetAvailableRolesByMemberId(id));
            Task.WaitAll(t1, t2, t3);

            var simpleMember = t1.Result;
            if (simpleMember == null) return null;
            simpleMember.Roles = t3.Result ?? new List<Role>();
            simpleMember.Permissions = t2.Result ?? new List<Permission>();

            return simpleMember;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResetPasswordResponse ResetPassword(ResetPasswordRequest request)
        {
            using (var db = DbFactory.GetClient())
            {
                var members = db.Queryable<Member>()
                    .Select(it => new Member {Id = it.Id, Password = it.Password, PasswordSalt = it.PasswordSalt})
                    .Where(it => request.MemberIds.Contains(it.Id))
                    .ToList();

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService
                    .CreatePasswordHash(request.NewPassword, saltKey, _memberSettings.HashedPasswordFormat);

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

                return new ResetPasswordResponse();
            }
        }


        public UpdateMemberResponse UpdateMember(UpdateMemberRequest request)
        {
            using (var db = DbFactory.GetClient())
            {
                var member = db.Queryable<Member>().InSingle(request.Id);
                if (member == null)
                    throw new MozException("找不到该用户");

                if (!member.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Username.Equals(request.Username))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                        throw new MozException($"此用户名 {request.Username} 已存在");

                    member.Username = request.Username;
                }

                if (!request.Password.Equals("******"))
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(request.Password, saltKey, _memberSettings.HashedPasswordFormat);

                    member.Password = newPassword;
                    member.PasswordSalt = saltKey;
                }

                if (!request.Email.IsNullOrEmpty() &&
                    !request.Email.Equals(member.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Email.Equals(request.Email))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                        throw new MozException($"此邮箱 {request.Email} 已存在");

                    member.Email = request.Email;
                }

                if (!request.Mobile.IsNullOrEmpty() &&
                    !request.Mobile.Equals(member.Mobile, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Member>()
                        .Where(it => it.Mobile.Equals(request.Mobile))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                        throw new MozException($"此手机 {request.Mobile} 已存在");

                    member.Mobile = request.Mobile;
                }

                member.Nickname = request.Nickname;
                member.Avatar = request.Avatar;
                member.Gender = request.Gender;
                member.Birthday = request.BirthDay;



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

                    if (request.Roles != null && request.Roles.Length > 0)
                    {
                        var newRoles = request.Roles.Select(it => new MemberRole
                        {
                            ExpireDate = null,
                            MemberId = member.Id,
                            RoleId = it
                        }).ToList();
                        tran.Insertable(newRoles).ExecuteCommand();
                    }

                    return 0;
                });
                
                _distributedCache.Remove($"cache_member_{member.Id}");

                return new UpdateMemberResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            using (var db = DbFactory.GetClient())
            {
                var member = db.Queryable<Member>().InSingle(request.MemberId);
                if (member == null)
                    throw new MozException("找不到该用户");
                
                var inputEncryptOldPassword = _encryptionService.CreatePasswordHash(request.OldPassword, member.PasswordSalt, _memberSettings.HashedPasswordFormat);
                if(!inputEncryptOldPassword.Equals(member.Password, StringComparison.OrdinalIgnoreCase)) 
                    throw new MozException("原密码不正确");
                
                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService
                    .CreatePasswordHash(request.NewPassword, saltKey, _memberSettings.HashedPasswordFormat);

                member.Password = newPassword;
                member.PasswordSalt = saltKey;

                db.Updateable(member).UpdateColumns(it => new
                {
                    it.Password,
                    it.PasswordSalt
                }).ExecuteCommand();
            }

            return new ChangePasswordResponse();
        }
        
        #endregion

        #region 角色权限

        /// <summary>
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<Role> GetAvailableRolesByMemberId(long memberId)
        {
            var dt = DateTime.UtcNow;
            using (var client = DbFactory.GetClient())
            {
                return client.Queryable<MemberRole, Role>((mr, r) => new object[]
                    {
                        JoinType.Left, mr.RoleId == r.Id
                    })
                    .Where((mr, r) =>
                        mr.MemberId == memberId
                        && (mr.ExpireDate == null || mr.ExpireDate != null && mr.ExpireDate > dt)
                        && r.IsActive)
                    .Select((mr, r) => new Role
                        {Id = r.Id, Code = r.Code, IsActive = r.IsActive, Name = r.Name, IsAdmin = r.IsAdmin})
                    .ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IEnumerable<Permission> GetAvailablePermissionsByMemeberId(long memberId)
        {
            var dt = DateTime.UtcNow;
            using (var client = DbFactory.GetClient())
            {
                var permissionsFromRoles = client.Queryable<MemberRole, RolePermisson, Permission>((mr, rp, p) =>
                        new object[]
                        {
                            JoinType.Left, mr.RoleId == rp.RoleId,
                            JoinType.Left, rp.PermissonId == p.Id
                        })
                    .Where((mr, rp, p) =>
                        mr.MemberId == memberId
                        && (mr.ExpireDate == null || mr.ExpireDate != null && mr.ExpireDate > dt)
                        && p.IsActive)
                    .Select((mr, rp, p) =>
                        new Permission {Id = p.Id, Code = p.Code, IsActive = p.IsActive, Name = p.Name})
                    .ToList();

                var permissionsFromMember = client.Queryable<MemberPermission, Permission>((mp, p) =>
                        new object[]
                        {
                            JoinType.Left, mp.PermissionId == p.Id
                        })
                    .Where((mp, p) => mp.MemberId == memberId && p.IsActive)
                    .Select((mp, p) =>
                        new Permission {Id = p.Id, Code = p.Code, IsActive = p.IsActive, Name = p.Name})
                    .ToList();

                permissionsFromRoles.AddRange(permissionsFromMember);
                return permissionsFromRoles.Distinct(new PermissionComparer()).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleId"></param>
        public bool TryAddRoleToMemeberId(long memberId, long roleId)
        {
            using (var client = DbFactory.GetClient())
            {
                var mRole = client.Queryable<MemberRole>().Single(t => t.MemberId == memberId && t.RoleId == roleId);
                if (mRole != null) return false;

                var id = client.Insertable(new MemberRole
                {
                    ExpireDate = new DateTime(2099, 1, 1),
                    MemberId = memberId,
                    RoleId = roleId
                }).ExecuteReturnBigIdentity();
                return id > 0;
            }
        }

        public PagedList<Permission> GetPermissionsPagedList(Expression<Func<Permission, Permission>> selectFun = null,
            Expression<Func<Permission, bool>> whereFun = null,
            KeyValuePair<Expression<Func<Permission, object>>, OrderByType>? orders = null,
            int pageIndex = 0,
            int pageSize = 2147483647)
        {
            throw new Exception("xx");
            /*
            return _repository.GetPagedList<Permission>(selectFun:selectFun,
                whereFun:whereFun,
                orders:orders,
                pageIndex:pageIndex, 
                pageSize:pageSize);
                */
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetRoleDetailResponse GetRoleDetail(GetRoleDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Id);
                if (role == null)
                {
                    return null;
                }

                var resp = new GetRoleDetailResponse
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code,
                    IsAdmin = role.IsAdmin,
                    IsActive = role.IsActive
                };

                return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateRoleResponse CreateRole(CreateRoleRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = new Role
                {
                    Name = request.Name,
                    IsActive = request.IsActive,
                    Code = request.Code,
                    IsAdmin = request.IsAdmin,
                    IsSystem = false
                };
                role.Id = client.Insertable(role).ExecuteReturnBigIdentity();

                //_cacheManager.RemoveOnEntityCreated<Role>();
                _eventPublisher.EntityCreated(role);

                return new CreateRoleResponse();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public UpdateRoleResponse UpdateRole(UpdateRoleRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Id);
                if (role == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    throw new MozException("不能编辑内置角色");
                }

                role.Name = request.Name;
                role.IsActive = request.IsActive;
                role.Code = request.Code;
                role.IsAdmin = request.IsAdmin;
                client.Updateable(role).ExecuteCommand();

                //_cacheManager.RemoveOnEntityUpdated<Role>(request.Id);
                _eventPublisher.EntityUpdated(role);

                return new UpdateRoleResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteRoleResponse DeleteRole(DeleteRoleRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Id);
                if (role == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    throw new MozException("不能删除内置角色");
                }

                client.Deleteable<Role>(request.Id).ExecuteCommand();

                //_cacheManager.RemoveOnEntityDeleted<Role>(request.Id);
                _eventPublisher.EntityDeleted(role);

                return new DeleteRoleResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryRoleResponse PagedQueryRoles(PagedQueryRoleRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Role>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t => new QueryRoleItem()
                    {
                        Id = t.Id,
                        Code = t.Code,
                        IsActive = t.IsActive,
                        IsAdmin = t.IsAdmin,
                        IsSystem = t.IsSystem,
                        Name = t.Name
                    })
                    .OrderBy("id ASC")
                    .ToPageList(page, pageSize, ref total);
                return new PagedQueryRoleResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SetRoleIsActiveResponse SetRoleIsActive(SetRoleIsActiveRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Id);
                if (role == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    throw new MozException("不能设置内置角色");
                }

                role.IsActive = request.IsActive;

                client.Updateable<Role>(role).UpdateColumns(t => new {t.IsActive}).ExecuteCommand();

                return new SetRoleIsActiveResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SetRoleIsAdminResponse SetRoleIsAdmin(SetRoleIsAdminRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Id);
                if (role == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    throw new MozException("不能设置内置角色");
                }

                role.IsAdmin = request.IsAdmin;

                client.Updateable<Role>(role).UpdateColumns(t => new {t.IsAdmin}).ExecuteCommand();

                return new SetRoleIsAdminResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetPermissionsByRoleResponse GetPermissionsByRole(GetPermissionsByRoleRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RolePermisson, Permission>((rp, p) => new object[]
                    {
                        JoinType.Left, rp.PermissonId == p.Id
                    })
                    .Where((rp, p) => rp.RoleId == request.RoleId)
                    .Select((rp, p) => new Permission()
                    {
                        Code = p.Code,
                        Id = p.Id,
                        IsActive = p.IsActive,
                        Name = p.Name,
                        ParentId = p.ParentId
                    })
                    .OrderBy("order_index ASC, id ASC")
                    .ToList();

                return new GetPermissionsByRoleResponse()
                {
                    Permissions = list
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ConfigPermissionResponse ConfigPermission(ConfigPermissionRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RolePermisson, Permission>((rp, p) => new object[]
                    {
                        JoinType.Left, rp.PermissonId == p.Id
                    })
                    .Where((rp, p) => rp.RoleId == request.RoleId)
                    .Select((rp, p) => new
                    {
                        PermissionId = p.Id,
                        RolePermissonId = rp.Id
                    })
                    .ToList();

                client.UseTran(tran =>
                {
                    var willAddPermissions = request.ConfigedPermissions
                        .Where(t => list.All(x => x.PermissionId != t.Id))
                        .Select(t => new RolePermisson()
                        {
                            PermissonId = t.Id,
                            RoleId = request.RoleId
                        })
                        .ToList();

                    var willRemovePermissions = list
                        .Where(t => request.ConfigedPermissions.All(x => x.Id != t.PermissionId))
                        .Select(t => t.RolePermissonId)
                        .ToList();

                    if (willAddPermissions.Any())
                        tran.Insertable<RolePermisson>(willAddPermissions).ExecuteCommand();

                    if (willRemovePermissions.Any())
                        tran.Deleteable<RolePermisson>().In(willRemovePermissions).ExecuteCommand();

                    return 0;
                });

                return new ConfigPermissionResponse();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ConfigMenuResponse ConfigMenu(ConfigMenuRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RoleMenu, AdminMenu>((rm, m) => new object[]
                    {
                        JoinType.Left, rm.MenuId == m.Id
                    })
                    .Where((rm, m) => rm.RoleId == request.RoleId)
                    .Select((rm, m) => new
                    {
                        MenuId = m.Id,
                        RoleMenuId = rm.Id
                    })
                    .ToList();

                client.UseTran(tran =>
                {
                    var willAddMenus = request.ConfigedMenus
                        .Where(t => list.All(x => x.MenuId != t.Id))
                        .Select(t => new RoleMenu()
                        {
                            MenuId = t.Id,
                            RoleId = request.RoleId
                        })
                        .ToList();

                    var willRemoveMenus = list
                        .Where(t => request.ConfigedMenus.All(x => x.Id != t.MenuId))
                        .Select(t => t.RoleMenuId)
                        .ToList();

                    if (willAddMenus.Any())
                        tran.Insertable<RoleMenu>(willAddMenus).ExecuteCommand();

                    if (willRemoveMenus.Any())
                        tran.Deleteable<RoleMenu>().In(willRemoveMenus).ExecuteCommand();

                    return 0;
                });

                return new ConfigMenuResponse();
            }
        }


        #endregion

        #region 权限管理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetPermissionDetailResponse GetPermissionDetail(GetPermissionDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var permission = client.Queryable<Permission>().InSingle(request.Id);
                if (permission == null)
                {
                    return null;
                }

                var resp = new GetPermissionDetailResponse();
                resp.Id = permission.Id;
                resp.Code = permission.Code;
                resp.IsActive = permission.IsActive;
                resp.Name = permission.Name;
                resp.ParentId = permission.ParentId;

                return resp;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreatePermissionResponse CreatePermission(CreatePermissionRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var entity = new Permission();
                entity.Name = request.Name;
                entity.Code = request.Code;
                entity.IsActive = request.IsActive;
                entity.ParentId = request.ParentId;
                entity.Id = client.Insertable(entity).ExecuteReturnBigIdentity();

                //_cacheManager.RemoveOnEntityCreated<Permission>();
                _eventPublisher.EntityCreated(entity);

                return new CreatePermissionResponse();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public UpdatePermissionResponse UpdatePermission(UpdatePermissionRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var permission = client.Queryable<Permission>().InSingle(request.Id);
                if (permission == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (permission.IsSystem)
                {
                    throw new MozException("不能编辑内置权限");
                }

                permission.Name = request.Name;
                permission.Code = request.Code;
                permission.IsActive = request.IsActive;
                permission.ParentId = request.ParentId;
                permission.OrderIndex = request.OrderIndex;
                client.Updateable(permission).ExecuteCommand();

                //_cacheManager.RemoveOnEntityUpdated<Permission>(request.Id);
                _eventPublisher.EntityUpdated(permission);

                return new UpdatePermissionResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeletePermissionResponse DeletePermission(DeletePermissionRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var permission = client.Queryable<Permission>().InSingle(request.Id);
                if (permission == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (permission.IsSystem)
                {
                    throw new MozException("不能删除内置权限");
                }

                client.Deleteable<Permission>(request.Id).ExecuteCommand();

                //_cacheManager.RemoveOnEntityDeleted<Permission>(request.Id);
                _eventPublisher.EntityDeleted(permission);

                return new DeletePermissionResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BulkDeletePermissionsResponse BulkDeletePermissions(BulkDeletePermissionsRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<Permission>().In(request.Ids).ExecuteCommand();

                request.Ids.ToList().ForEach(t =>
                {
                    //_cacheManager.RemoveOnEntityDeleted<Permission>(t);
                });
                _eventPublisher.EntitiesDeleted<Permission>(request.Ids);

                return new BulkDeletePermissionsResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryPermissionResponse PagedQueryPermissions(PagedQueryPermissionRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Permission>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t => new QueryPermissionItem()
                    {
                        Code = t.Code,
                        Id = t.Id,
                        IsActive = t.IsActive,
                        Name = t.Name,
                        ParentId = t.ParentId,
                        OrderIndex = t.OrderIndex,
                        IsSystem = t.IsSystem
                    })
                    .OrderBy("order_index ASC,id ASC")
                    .ToPageList(page, pageSize, ref total);
                return new PagedQueryPermissionResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public SetPermissionIsActiveResponse SetPermissionIsActive(SetPermissionIsActiveRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var permission = client.Queryable<Permission>().InSingle(request.Id);
                if (permission == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (permission.IsSystem)
                {
                    throw new MozException("不能设置内置权限");
                }

                permission.IsActive = request.IsActive;

                client.Updateable<Permission>(permission).UpdateColumns(t => new {t.IsActive}).ExecuteCommand();

                return new SetPermissionIsActiveResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public SetPermissionOrderIndexResponse SetPermissionOrderIndex(SetPermissionOrderIndexRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var permission = client.Queryable<Permission>().InSingle(request.Id);
                if (permission == null)
                {
                    throw new MozException("找不到该条信息");
                }

                permission.OrderIndex = request.OrderIndex;

                client.Updateable<Permission>(permission).UpdateColumns(t => new {t.OrderIndex}).ExecuteCommand();

                return new SetPermissionOrderIndexResponse();
            }
        }

        #endregion

    }
}