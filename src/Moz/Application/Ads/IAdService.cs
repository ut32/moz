using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdPlaces;
using Moz.Bus.Dtos.Ads;

namespace Moz.Bus.Services.Ads
{
    public interface IAdService
    {
        #region 广告位置
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult CreateAdPlace(CreateAdPlaceDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateAdPlace(UpdateAdPlaceDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult DeleteAdPlace(DeleteAdPlaceDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult BulkDeleteAdPlaces(BulkDeleteAdPlacesDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<GetAdPlaceDetailInfo> GetAdPlaceDetail(GetAdPlaceDetailDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryAdPlaceItem>> PagedQueryAdPlaces(PagedQueryAdPlaceDto dto); 
        
        #endregion
        
        #region 广告

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult CreateAd(CreateAdDto dto); 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateAd(UpdateAdDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult DeleteAd(DeleteAdDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult BulkDeleteAds(BulkDeleteAdsDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<GetAdDetailInfo> GetAdDetail(GetAdDetailDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryAdItem>> PagedQueryAds(PagedQueryAdsDto dto); 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetAdOrder(SetAdOrderDto dto);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetAdIsShow(SetAdIsShowDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<GetAdsByCodeInfo> GetAdsByCode(GetAdsByCodeDto dto); 

        #endregion
    }
}