using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moz.Biz.Dtos.Members;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Dtos.Members.Permissions;
using Moz.Bus.Dtos.Members.Roles;
using Moz.Bus.Models.Members;
using Moz.Domain.Dtos.Members.Permissions;
using Moz.Domain.Dtos.Members.Roles;
using Moz.Utils.Impl;
using SqlSugar;

namespace Moz.Bus.Services.Members
{
    public interface IMemberService
    {
        #region 会员管理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        SimpleMember GetSimpleMemberByUId(string uid); 

        /// <summary> 
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ResetPasswordResponse ResetPassword(ResetPasswordRequest request);

        /// <summary>
        /// 获取用户详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetMemberDetailResponse GetMemberDetail(GetMemberDetailRequest request);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UpdateMemberResponse UpdateMember(UpdateMemberRequest request);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);
        
        #endregion
        
        #region 角色管理
        
        CreateRoleResponse CreateRole(CreateRoleRequest request);
        UpdateRoleResponse UpdateRole(UpdateRoleRequest request);
        DeleteRoleResponse DeleteRole(DeleteRoleRequest request);
        GetRoleDetailResponse GetRoleDetail(GetRoleDetailRequest request);
        PagedQueryRoleResponse PagedQueryRoles(PagedQueryRoleRequest request);
        GetPermissionsByRoleResponse GetPermissionsByRole(GetPermissionsByRoleRequest request);
        ConfigPermissionResponse ConfigPermission(ConfigPermissionRequest request);
        SetRoleIsActiveResponse SetRoleIsActive(SetRoleIsActiveRequest request);
        SetRoleIsAdminResponse SetRoleIsAdmin(SetRoleIsAdminRequest request);
        ConfigMenuResponse ConfigMenu(ConfigMenuRequest request);
        
        #endregion 

        #region 权限管理

        CreatePermissionResponse CreatePermission(CreatePermissionRequest request);
        UpdatePermissionResponse UpdatePermission(UpdatePermissionRequest request);
        DeletePermissionResponse DeletePermission(DeletePermissionRequest request);
        BulkDeletePermissionsResponse BulkDeletePermissions(BulkDeletePermissionsRequest request);
        GetPermissionDetailResponse GetPermissionDetail(GetPermissionDetailRequest request);
        PagedQueryPermissionResponse PagedQueryPermissions(PagedQueryPermissionRequest request);
        SetPermissionIsActiveResponse SetPermissionIsActive(SetPermissionIsActiveRequest request);
        SetPermissionOrderIndexResponse SetPermissionOrderIndex(SetPermissionOrderIndexRequest request);

        #endregion

        #region MyRegion

        PagedQueryMemberResponse PagedQueryMembers(PagedQueryMemberRequest request);

        #endregion
    }
}