using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdminMenus;

namespace Moz.Bus.Services.AdminMenus
{
    public interface IAdminMenuService
    {
        ServResult CreateAdminMenu(ServRequest<CreateAdminMenuDto> request);
        ServResult UpdateAdminMenu(ServRequest<UpdateAdminMenuDto> request);
        ServResult DeleteAdminMenu(ServRequest<DeleteAdminMenuDto> request);
        ServResult SetAdminMenuOrderIndex(ServRequest<SetAdminMenuOrderIndexDto> request);
        ServResult<GetAdminMenuDetailApo> GetAdminMenuDetail(ServRequest<GetAdminMenuDetailDto> request);
        ServResult<PagedList<QueryAdminMenuItem>> PagedQueryAdminMenus(ServRequest<PagedQueryAdminMenusDto> request);
        //ServResult<GetMenusByRoleApo> GetMenusByRole(ServRequest<GetMenusByRoleDto> request);
        ServResult<QueryChildrenByParentIdApo> QueryChildrenByParentId(ServRequest<QueryChildrenByParentIdDto> request);
    }
}