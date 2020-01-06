using System.Collections.Generic;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Common;

namespace Moz.Domain.Services.AdminMenus
{
    public interface IAdminMenuService
    {
        CreateAdminMenuResponse CreateAdminMenu(CreateAdminMenuRequest request);
        UpdateAdminMenuResponse UpdateAdminMenu(UpdateAdminMenuRequest request);
        DeleteAdminMenuResponse DeleteAdminMenu(DeleteAdminMenuRequest request);
        SetAdminMenuOrderIndexResponse SetAdminMenuOrderIndex(SetAdminMenuOrderIndexRequest request);
        GetAdminMenuDetailResponse GetAdminMenuDetail(GetAdminMenuDetailRequest request);
        PagedQueryAdminMenuResponse PagedQueryAdminMenus(PagedQueryAdminMenuRequest request);
        GetMenusByRoleResponse GetMenusByRole(GetMenusByRoleRequest request);
        QueryChildrenByParentIdResponse QueryChildrenByParentId(QueryChildrenByParentIdRequest request);
    }
}