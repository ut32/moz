using System.Collections.Generic;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Permissions;
using Moz.Bus.Models.Members;

namespace Moz.Bus.Services.Permissions
{
    public interface IPermissionService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult CreatePermission(ServRequest<CreatePermissionDto> request);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult UpdatePermission(ServRequest<UpdatePermissionDto> request);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult DeletePermission(ServRequest<DeletePermissionDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetOrderIndex(ServRequest<SetOrderIndexPermissionDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetIsActive(ServRequest<SetIsActivePermissionDto> request);
        
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<PermissionDetailApo> GetPermissionDetail(ServRequest<GetPermissionDetailDto> request);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<PagedList<QueryPermissionItem>> PagedQueryPermissions(ServRequest<PagedQueryPermissionDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<List<PermissionTree>> QuerySubPermissionsByParentId(ServRequest<long?> request);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ServResult<List<AvailablePermission>> GetAvailablePermissions(); 
    }
}