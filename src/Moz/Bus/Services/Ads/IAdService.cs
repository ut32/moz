using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdPlaces;
using Moz.Bus.Dtos.Ads;

namespace Moz.Bus.Services.Ads
{
    public interface IAdService
    {
        #region 广告位置
        
        ServResult CreateAdPlace(ServRequest<CreateAdPlaceDto> request);
        ServResult UpdateAdPlace(ServRequest<UpdateAdPlaceDto> request);
        ServResult DeleteAdPlace(ServRequest<DeleteAdPlaceDto> request);
        ServResult BulkDeleteAdPlaces(ServRequest<BulkDeleteAdPlacesDto> request);
        ServResult<GetAdPlaceDetailApo> GetAdPlaceDetail(ServRequest<GetAdPlaceDetailDto> request);
        ServResult<PagedList<QueryAdPlaceItem>> PagedQueryAdPlaces(ServRequest<PagedQueryAdPlaceDto> request);
        
        #endregion
        
        #region 广告

        ServResult CreateAd(ServRequest<CreateAdDto> request);
        ServResult UpdateAd(ServRequest<UpdateAdDto> request);
        ServResult DeleteAd(ServRequest<DeleteAdDto> request);
        ServResult BulkDeleteAds(ServRequest<BulkDeleteAdsDto> request);
        ServResult<GetAdDetailApo> GetAdDetail(ServRequest<GetAdDetailDto> request);
        ServResult<PagedList<QueryAdItem>> PagedQueryAds(ServRequest<PagedQueryAdsDto> request);

        ServResult SetAdOrder(ServRequest<SetAdOrderDto> request);

        ServResult SetAdIsShow(ServRequest<SetAdIsShowDto> request);

        ServResult<GetAdsByCodeApo> GetAdsByCode(ServRequest<GetAdsByCodeDto> request);

        #endregion
    }
}