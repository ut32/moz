using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdminMenus;

namespace Moz.Bus.Services.AdminMenus
{
    public interface IAdminMenuService
    {
        PublicResult CreateAdminMenu(CreateAdminMenuDto dto);
        PublicResult UpdateAdminMenu(UpdateAdminMenuDto dto);
        PublicResult DeleteAdminMenu(DeleteAdminMenuDto dto);
        PublicResult SetAdminMenuOrderIndex(SetAdminMenuOrderIndexDto dto);
        PublicResult<GetAdminMenuDetailInfo> GetAdminMenuDetail(GetAdminMenuDetailDto dto);
        PublicResult<PagedList<QueryAdminMenuItem>> PagedQueryAdminMenus(PagedQueryAdminMenusDto dto);
        PublicResult<QueryChildrenByParentIdApo> QueryChildrenByParentId(QueryChildrenByParentIdDto dto);
    }
}