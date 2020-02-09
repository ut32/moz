using System.Collections.Generic;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Roles;
using Moz.Bus.Models.Members;

namespace Moz.Bus.Services.Roles
{
    public interface IRoleService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult CreateRole(ServRequest<CreateRoleDto> request);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult UpdateRole(ServRequest<UpdateRoleDto> request);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult DeleteRole(ServRequest<DeleteRoleDto> request);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<RoleDetailApo> GetRoleDetail(ServRequest<GetRoleDetailDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetIsActive(ServRequest<SetIsActiveRoleDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetIsAdmin(ServRequest<SetIsAdminRoleDto> request);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<PagedList<QueryRoleItem>> PagedQueryRoles(ServRequest<PagedQueryRoleDto> request);

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<GetPermissionsByRoleApo> GetPermissionsByRole(ServRequest<GetPermissionsByRoleDto> request);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult ConfigPermission(ServRequest<ConfigPermissionDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<GetMenusByRoleApo> GetMenusByRole(ServRequest<GetMenusByRoleDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult ConfigMenu(ServRequest<ConfigMenuDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ServResult<List<Role>> GetAvailableRoles();
    }
}