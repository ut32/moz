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
                using (var client = DbFactory.GetClient())
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<RoleDetailApo> GetRoleDetail(ServRequest<GetRoleDetailDto> request)
        {
            Role role = null;
            using (var client = DbFactory.GetClient())
            {
                 role = client.Queryable<Role>().InSingle(request.Data.Id);
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult CreateRole(ServRequest<CreateRoleDto> request)
        {
            var role = new Role
            {
                Name = request.Data.Name,
                IsActive = request.Data.IsActive,
                Code = request.Data.Code,
                IsAdmin = request.Data.IsAdmin,
                IsSystem = false
            };
            using (var client = DbFactory.GetClient())
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult UpdateRole(ServRequest<UpdateRoleDto> request)
        {
            Role role = null;
            using (var client = DbFactory.GetClient())
            {
                role = client.Queryable<Role>().InSingle(request.Data.Id);
                if (role == null)
                {
                    return Error("找不到该条信息");
                }

                if (role.IsSystem)
                {
                    return Error("内置用户不能修改");
                }

                role.Name = request.Data.Name;
                role.IsActive = request.Data.IsActive;
                role.Code = request.Data.Code;
                role.IsAdmin = request.Data.IsAdmin;
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult DeleteRole(ServRequest<DeleteRoleDto> request)
        {
            Role role = null;
            using (var client = DbFactory.GetClient())
            {
                role = client.Queryable<Role>().InSingle(request.Data.Id);
                if (role == null)
                {
                    return Error("找不到该条信息");
                }
                if (role.IsSystem)
                {
                    return Error("内置用户不能删除");
                }
                client.Deleteable<Role>(request.Data.Id).ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            _eventPublisher.EntityDeleted(role);
            return Ok();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult SetIsActive(ServRequest<SetIsActiveRoleDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Data.Id);
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
                    .Where(it => it.Id == request.Data.Id)
                    .ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult SetIsAdmin(ServRequest<SetIsAdminRoleDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var role = client.Queryable<Role>().InSingle(request.Data.Id);
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
                    .Where(it => it.Id == request.Data.Id)
                    .ExecuteCommand();
            }
            _distributedCache.Remove(PermissionService.CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_ROLE_ALL_KEY);
            return Ok();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<PagedList<QueryRoleItem>> PagedQueryRoles(ServRequest<PagedQueryRoleDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Role>()
                    .WhereIF(!string.IsNullOrEmpty(request.Data.Keyword), t => t.Name.Contains(request.Data.Keyword))
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetPermissionsByRoleApo> GetPermissionsByRole(ServRequest<GetPermissionsByRoleDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RolePermission, Permission>((rp, p) => new object[]
                    {
                        JoinType.Left, rp.PermissionId == p.Id
                    })
                    .Where((rp, p) => rp.RoleId == request.Data.RoleId)
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult ConfigPermission(ServRequest<ConfigPermissionDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RolePermission, Permission>((rp, p) => new object[]
                    {
                        JoinType.Left, rp.PermissionId == p.Id
                    })
                    .Where((rp, p) => rp.RoleId == request.Data.RoleId)
                    .Select((rp, p) => new
                    {
                        PermissionId = p.Id,
                        RolePermissonId = rp.Id
                    })
                    .ToList();

                client.UseTran(tran =>
                {
                    var willAddPermissions = request.Data.ConfigedPermissions
                        .Where(t => list.All(x => x.PermissionId != t.Id))
                        .Select(t => new RolePermission()
                        {
                            PermissionId = t.Id,
                            RoleId = request.Data.RoleId
                        })
                        .ToList();

                    var willRemovePermissions = list
                        .Where(t => request.Data.ConfigedPermissions.All(x => x.Id != t.PermissionId))
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetMenusByRoleApo> GetMenusByRole(ServRequest<GetMenusByRoleDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RoleMenu, AdminMenu>((rm, m) => new object[]
                    {
                        JoinType.Left, rm.MenuId==m.Id
                    })
                    .Where((rm, m) => rm.RoleId == request.Data.RoleId)
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult ConfigMenu(ServRequest<ConfigMenuDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RoleMenu, AdminMenu>((rm, m) => new object[]
                    {
                        JoinType.Left, rm.MenuId == m.Id
                    })
                    .Where((rm, m) => rm.RoleId == request.Data.RoleId)
                    .Select((rm, m) => new
                    {
                        MenuId = m.Id,
                        RoleMenuId = rm.Id
                    })
                    .ToList();

                client.UseTran(tran =>
                {
                    var willAddMenus = request.Data.ConfigedMenus
                        .Where(t => list.All(x => x.MenuId != t.Id))
                        .Select(t => new RoleMenu()
                        {
                            MenuId = t.Id,
                            RoleId = request.Data.RoleId
                        })
                        .ToList();

                    var willRemoveMenus = list
                        .Where(t => request.Data.ConfigedMenus.All(x => x.Id != t.MenuId))
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
        public ServResult<List<Role>> GetAvailableRoles()
        {
            var list = GetRolesListCached();
            return list.Where(it => it.IsActive).ToList();
        }

        #endregion 
    }
}