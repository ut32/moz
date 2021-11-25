using System.Collections.Generic;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Roles;
using Moz.Bus.Models.Members;
using Moz.Model;

namespace Moz.Bus.Services.Roles
{
    public interface IRoleService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult CreateRole(CreateRoleDto dto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateRole(UpdateRoleDto dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult DeleteRole(DeleteRoleDto dto);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<RoleDetailApo> GetRoleDetail(GetRoleDetailDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetIsActive(SetIsActiveRoleDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetIsAdmin(SetIsAdminRoleDto dto);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryRoleItem>> PagedQueryRoles(PagedQueryRoleDto dto);

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<GetPermissionsByRoleApo> GetPermissionsByRole(GetPermissionsByRoleDto dto);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult ConfigPermission(ConfigPermissionDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<GetMenusByRoleApo> GetMenusByRole(GetMenusByRoleDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult ConfigMenu(ConfigMenuDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PublicResult<List<Role>> GetAvailableRoles();
    }
}