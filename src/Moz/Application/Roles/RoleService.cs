using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Bus.Dtos.Roles;
using Moz.Bus.Models.AdminMenus;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Permissions;
using Moz.DataBase;
using Moz.Events;
using Moz.Model;
using SqlSugar;
using GetMenusByRoleApo = Moz.Bus.Dtos.Roles.GetMenusByRoleApo;
using GetMenusByRoleDto = Moz.Bus.Dtos.Roles.GetMenusByRoleDto;

namespace Moz.Bus.Services.Roles
{
    public partial class RoleService : BaseService,IRoleService
    {
        #region Constants
        
        public static readonly string CACHE_ROLE_ALL_KEY = "CACHE_ROLE_ALL_KEY";
        
        #endregion 

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Ctor
        public RoleService(
            IEventPublisher eventPublisher,
            IDistributedCache distributedCache)
        {
            _eventPublisher = eventPublisher;
            _distributedCache = distributedCache;
        }
        #endregion

        #region Utils
        
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <returns></returns>
        private List<Role> GetRolesListCached()
        {
            return _distributedCache.GetOrSet(CACHE_ROLE_ALL_KEY, () =>
            {
                using (var client = DbFactory.CreateClient())
                {
                    return client.Queryable<Role>().OrderBy("id ASC").ToList();
                }
            });
        }
        

        #endregion

        #region Methods

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<RoleDetailApo> GetRoleDetail(GetRoleDetailDto dto)
        {
            Role role = null;
            using (var client = DbFactory.CreateClient())
            {
                 role = client.Queryable<Role>().InSingle(dto.Id);
            }
            if(role == null)
            {
                return Error("找不到数据");
            }

            var res = new RoleDetailApo
            {
                Id = role.Id,
                Name = role.Name,
                IsActive = role.IsActive,
                Code = role.Code,
                IsAdmin = role.IsAdmin,
                IsSystem = role.IsSystem
            };
            return res;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreateRole(CreateRoleDto dto)
        {
            var role = new Role
            {
                Name = dto.Name,
                IsActive = dto.IsActive,
                Code = dto.Code,
                IsAdmin = dto.IsAdmin,
                IsSystem = false
            };
            using (var client = DbFactory.CreateClient())
            {
                role.Id = client.Insertable(role).ExecuteReturnBigIdentity();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            _eventPublisher.EntityCreated(role);
            return Ok();
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateRole(UpdateRoleDto dto)
        {
            Role role = null;
            using (var client = DbFactory.CreateClient())
            {
                role = client.Queryable<Role>().InSingle(dto.Id);
                if (role == null)
                {
                    return Error("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    return Error("内置用户不能修改");
                }

                role.Name = dto.Name;
                role.IsActive = dto.IsActive;
                role.Code = dto.Code;
                role.IsAdmin = dto.IsAdmin;
                client.Updateable(role).ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            _eventPublisher.EntityUpdated(role);
            return Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeleteRole(DeleteRoleDto dto)
        {
            Role role = null;
            using (var client = DbFactory.CreateClient())
            {
                role = client.Queryable<Role>().InSingle(dto.Id);
                if (role == null)
                {
                    return Error("找不到该条信息");
                }
                if (role.IsSystem)
                {
                    return Error("内置用户不能删除");
                }
                client.Deleteable<Role>(dto.Id).ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            _eventPublisher.EntityDeleted(role);
            return Ok();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetIsActive(SetIsActiveRoleDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var role = client.Queryable<Role>().InSingle(dto.Id);
                if (role == null)
                {
                    return Error("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    return Error("内置用户不能删除");
                }

                client.Updateable<Role>()
                    .SetColumns(it => new Role()
                    {
                        IsActive = !it.IsActive
                    })
                    .Where(it => it.Id == dto.Id)
                    .ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetIsAdmin(SetIsAdminRoleDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var role = client.Queryable<Role>().InSingle(dto.Id);
                if (role == null)
                {
                    return Error("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    return Error("内置用户不能删除");
                }

                client.Updateable<Role>()
                    .SetColumns(it => new Role()
                    {
                        IsAdmin = !it.IsAdmin
                    })
                    .Where(it => it.Id == dto.Id)
                    .ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            return Ok();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryRoleItem>> PagedQueryRoles(PagedQueryRoleDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<Role>()
                    .WhereIF(!string.IsNullOrEmpty(dto.Keyword), t => t.Name.Contains(dto.Keyword))
                    .Select(t=>new QueryRoleItem()
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        IsActive = t.IsActive, 
                        Code = t.Code, 
                        IsAdmin = t.IsAdmin, 
                        IsSystem = t.IsSystem, 
                    })
                    .OrderBy("id ASC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedList<QueryRoleItem>
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetPermissionsByRoleApo> GetPermissionsByRole(GetPermissionsByRoleDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var list = client.Queryable<RolePermission, Permission>((rp, p) => new object[]
                    {
                        JoinType.Left, rp.PermissionId == p.Id
                    })
                    .Where((rp, p) => rp.RoleId == dto.RoleId)
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

                return new GetPermissionsByRoleApo()
                {
                    Permissions = list
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ConfigPermission(ConfigPermissionDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var list = client.Queryable<RolePermission, Permission>((rp, p) => new object[]
                    {
                        JoinType.Left, rp.PermissionId == p.Id
                    })
                    .Where((rp, p) => rp.RoleId == dto.RoleId)
                    .Select((rp, p) => new
                    {
                        PermissionId = p.Id,
                        RolePermissonId = rp.Id
                    })
                    .ToList();

                client.UseTran(tran =>
                {
                    var willAddPermissions = dto.ConfigedPermissions
                        .Where(t => list.All(x => x.PermissionId != t.Id))
                        .Select(t => new RolePermission()
                        {
                            PermissionId = t.Id,
                            RoleId = dto.RoleId
                        })
                        .ToList();

                    var willRemovePermissions = list
                        .Where(t => dto.ConfigedPermissions.All(x => x.Id != t.PermissionId))
                        .Select(t => t.RolePermissonId)
                        .ToList();

                    if (willAddPermissions.Any())
                        tran.Insertable<RolePermission>(willAddPermissions).ExecuteCommand();

                    if (willRemovePermissions.Any())
                        tran.Deleteable<RolePermission>().In(willRemovePermissions).ExecuteCommand();

                    return 0;
                });
                _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetMenusByRoleApo> GetMenusByRole(GetMenusByRoleDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var list = client.Queryable<RoleMenu, AdminMenu>((rm, m) => new object[]
                    {
                        JoinType.Left, rm.MenuId==m.Id
                    })
                    .Where((rm, m) => rm.RoleId == dto.RoleId)
                    .Select((rm, m) => new AdminMenu()
                    {
                        Icon = m.Icon,
                        Id = m.Id,
                        IsSystem = m.IsSystem,
                        Link = m.Link,
                        Name = m.Name,
                        OrderIndex = m.OrderIndex,
                        ParentId = m.ParentId
                    })
                    .OrderBy("order_index ASC, id ASC")
                    .ToList();
                
                return new GetMenusByRoleApo()
                {
                    Menus = list
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ConfigMenu(ConfigMenuDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var list = client.Queryable<RoleMenu, AdminMenu>((rm, m) => new object[]
                    {
                        JoinType.Left, rm.MenuId == m.Id
                    })
                    .Where((rm, m) => rm.RoleId == dto.RoleId)
                    .Select((rm, m) => new
                    {
                        MenuId = m.Id,
                        RoleMenuId = rm.Id
                    })
                    .ToList();

                client.UseTran(tran =>
                {
                    var willAddMenus = dto.ConfigedMenus
                        .Where(t => list.All(x => x.MenuId != t.Id))
                        .Select(t => new RoleMenu()
                        {
                            MenuId = t.Id,
                            RoleId = dto.RoleId
                        })
                        .ToList();

                    var willRemoveMenus = list
                        .Where(t => dto.ConfigedMenus.All(x => x.Id != t.MenuId))
                        .Select(t => t.RoleMenuId)
                        .ToList();

                    if (willAddMenus.Any())
                        tran.Insertable<RoleMenu>(willAddMenus).ExecuteCommand();

                    if (willRemoveMenus.Any())
                        tran.Deleteable<RoleMenu>().In(willRemoveMenus).ExecuteCommand();

                    return 0;
                });

                return Ok();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PublicResult<List<Role>> GetAvailableRoles()
        {
            var list = GetRolesListCached();
            return list.Where(it => it.IsActive).ToList();
        }

        #endregion 
    }
}