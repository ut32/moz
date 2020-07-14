using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Categories;
using Moz.Bus.Dtos.Permissions;
using Moz.Bus.Models.Categories;
using Moz.Bus.Models.Members;
using Moz.DataBase;
using Moz.Events;
using Moz.TaskSchedule;
using SqlSugar;

namespace Moz.Bus.Services.Permissions
{
    public partial class PermissionService : BaseService,IPermissionService
    {
        #region Constants

        public static readonly string CACHE_PERMISSION_ALL_KEY = "CACHE_PERMISSION_ALL_KEY";
        public static readonly string CACHE_ROLE_PERMISSION_ALL_KEY = "CACHE_ROLE_PERMISSION_ALL_KEY";
        #endregion

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Ctor
        public PermissionService(
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
        private List<Permission> GetPermissionsListCached()
        {
            return _distributedCache.GetOrSet(CACHE_PERMISSION_ALL_KEY, () =>
            {
                using (var client = DbFactory.CreateClient())
                {
                    return client.Queryable<Permission>().OrderBy("id ASC").ToList();
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
        public PublicResult<PermissionDetailApo> GetPermissionDetail(GetPermissionDetailDto dto)
        {
            Permission permission = null;
            using (var client = DbFactory.CreateClient())
            {
                 permission = client.Queryable<Permission>().InSingle(dto.Id);
            }
            if(permission == null)
            {
                return null;
            }

            var res = new PermissionDetailApo
            {
                Id = permission.Id,
                Name = permission.Name,
                Code = permission.Code,
                IsActive = permission.IsActive,
                ParentId = permission.ParentId,
                OrderIndex = permission.OrderIndex,
                IsSystem = permission.IsSystem
            };
            return res;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreatePermission(CreatePermissionDto dto)
        {
            var permission = new Permission
            {
                Name = dto.Name, 
                Code = dto.Code,
                IsActive = dto.IsActive,
                ParentId = dto.ParentId,
                OrderIndex = 0,
                IsSystem = false
            };
            using (var client = DbFactory.CreateClient())
            {
                permission.Id = client.Insertable(permission).ExecuteReturnBigIdentity();
            }
            _distributedCache.Remove(CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_PERMISSION_ALL_KEY);
            _eventPublisher.EntityCreated(permission);
            return Ok();
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdatePermission(UpdatePermissionDto dto)
        {
            Permission permission = null;
            using (var client = DbFactory.CreateClient())
            {
                permission = client.Queryable<Permission>().InSingle(dto.Id);
                if (permission == null)
                {
                    return Error("找不到该条信息");
                }

                if (permission.IsSystem)
                {
                    return Error("内置信息不能修改");
                }

                permission.Name = dto.Name;
                permission.Code = dto.Code;
                permission.IsActive = dto.IsActive;
                permission.ParentId = dto.ParentId;
                client.Updateable(permission).ExecuteCommand();
            }
            _distributedCache.Remove(CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_PERMISSION_ALL_KEY);
            _eventPublisher.EntityUpdated(permission);
            return Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeletePermission(DeletePermissionDto dto)
        {
            Permission permission = null;
            using (var client = DbFactory.CreateClient())
            {
                permission = client.Queryable<Permission>().InSingle(dto.Id);
                if (permission == null)
                {
                    return Error("找不到该条信息");
                }
                if (permission.IsSystem)
                {
                    return Error("内置信息不能删除");
                }
                client.Deleteable<Permission>(dto.Id).ExecuteCommand();
            }
            _distributedCache.Remove(CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_PERMISSION_ALL_KEY);
            _eventPublisher.EntityDeleted(permission);
            return Ok();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetOrderIndex(SetOrderIndexPermissionDto dto)
        {
            Permission permission = null;
            using (var client = DbFactory.CreateClient())
            {
                permission = client.Queryable<Permission>().InSingle(dto.Id);
                if (permission == null)
                {
                    return Error("找不到该条信息");
                }
                if (permission.IsSystem)
                {
                    return Error("内置信息不能修改");
                }
                client.Updateable<Permission>().SetColumns(it=>new Permission
                {
                    OrderIndex = dto.OrderIndex
                }).Where(it=>it.Id==dto.Id).ExecuteCommand();
            }
            _eventPublisher.EntityDeleted(permission);
            return Ok();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetIsActive(SetIsActivePermissionDto dto)
        {
            Permission permission = null;
            using (var client = DbFactory.CreateClient())
            {
                permission = client.Queryable<Permission>().InSingle(dto.Id);
                if (permission == null)
                {
                    return Error("找不到该条信息");
                }
                if (permission.IsSystem)
                {
                    return Error("内置信息不能修改");
                }
                client.Updateable<Permission>().SetColumns(it=>new Permission
                {
                    IsActive = !it.IsActive
                }).Where(it=>it.Id==dto.Id).ExecuteCommand();
            }
            _distributedCache.Remove(CACHE_ROLE_PERMISSION_ALL_KEY);
            _distributedCache.Remove(CACHE_PERMISSION_ALL_KEY);
            _eventPublisher.EntityDeleted(permission);
            return Ok();
        }

        

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryPermissionItem>> PagedQueryPermissions(PagedQueryPermissionDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<Permission>()
                    //.WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryPermissionItem()
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        Code = t.Code, 
                        IsActive = t.IsActive, 
                        ParentId = t.ParentId, 
                        OrderIndex = t.OrderIndex, 
                        IsSystem = t.IsSystem, 
                    })
                    .OrderBy("order_index ASC,id ASC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedList<QueryPermissionItem>
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
        /// <returns></returns>
        public PublicResult<List<AvailablePermission>> GetAvailablePermissions()
        {
            return _distributedCache.GetOrSet(CACHE_ROLE_PERMISSION_ALL_KEY, () =>
            {
                using (var client = DbFactory.CreateClient())
                {
                    return client.Queryable<RolePermission, Role, Permission>(
                            (rp, r, p) => new object[]
                            {
                                JoinType.Left, rp.RoleId == r.Id,
                                JoinType.Left, rp.PermissionId == p.Id
                            })
                        .Where((rp, r, p) => r.IsActive && p.IsActive)
                        .Select((rp, r, p) => new AvailablePermission
                        {
                            Code = p.Code,
                            Id = p.Id,
                            Name = p.Name,
                            RoleId = r.Id,
                            RoleCode = r.Code
                        })
                        .ToList();
                }
            });
        }


        /// <summary>
        /// 查询所有子类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public PublicResult<List<PermissionTree>> QuerySubPermissionsByParentId(long? parentId)
        {
            return GetAllSubPermissions(parentId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<PermissionTree> GetAllSubPermissions(long? parentId)
        {
            var list = GetPermissionsListCached(); 
            var subPermissions = list.Where(t => t.ParentId == parentId).ToList();
            var result = new List<PermissionTree>();
            foreach (var subPermission in subPermissions)
            {
                result.Add(new PermissionTree()
                {
                    Id = subPermission.Id,
                    Name = subPermission.Name,
                    Alias = "",
                    Children = GetAllSubPermissions(subPermission.Id)
                });   
            }
            return result;
        }


        #endregion
    }
}