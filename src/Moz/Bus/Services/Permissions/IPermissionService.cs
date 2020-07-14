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
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult CreatePermission(CreatePermissionDto dto);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdatePermission(UpdatePermissionDto dto); 
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult DeletePermission(DeletePermissionDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetOrderIndex(SetOrderIndexPermissionDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetIsActive(SetIsActivePermissionDto dto);
        
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PermissionDetailApo> GetPermissionDetail(GetPermissionDetailDto dto);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryPermissionItem>> PagedQueryPermissions(PagedQueryPermissionDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<List<PermissionTree>> QuerySubPermissionsByParentId(long? dto);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PublicResult<List<AvailablePermission>> GetAvailablePermissions(); 
    }
}