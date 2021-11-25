using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdPlaces;
using Moz.Bus.Dtos.Ads;
using Moz.Bus.Models.Ads;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;
using SqlSugar;

namespace Moz.Bus.Services.Ads
{
    public partial class AdService : BaseService, IAdService
    {
        #region Constants

        #endregion

        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;

        #endregion

        #region Ctor

        public AdService(
            IEventPublisher eventPublisher,
            IDistributedCache distributedCache)
        {
            _eventPublisher = eventPublisher;
            _distributedCache = distributedCache;
        }

        #endregion

        #region 广告

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetAdDetailInfo> GetAdDetail(GetAdDetailDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var ad = client.Queryable<Ad>().InSingle(dto.Id);
                if (ad == null)
                {
                    return Error("找不到此信息");
                }

                var resp = new GetAdDetailInfo
                {
                    Id = ad.Id,
                    AdPlaceId = ad.AdPlaceId,
                    Title = ad.Title,
                    ImagePath = ad.ImagePath,
                    TargetUrl = ad.TargetUrl,
                    Order = ad.OrderIndex,
                    IsShow = ad.IsShow
                };
                return resp;
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreateAd(CreateAdDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var ad = new Ad
                {
                    AdPlaceId = dto.AdPlaceId,
                    Title = dto.Title,
                    ImagePath = dto.ImagePath,
                    TargetUrl = dto.TargetUrl,
                    OrderIndex = 0,
                    IsShow = true
                };
                ad.Id = client.Insertable(ad).ExecuteReturnBigIdentity();

                _eventPublisher.EntityCreated(ad);
                return Ok();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateAd(UpdateAdDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var ad = client.Queryable<Ad>().InSingle(dto.Id);
                if (ad == null)
                {
                    return Error("找不到该条信息");
                }
                
                ad.Title = dto.Title;
                ad.ImagePath = dto.ImagePath;
                ad.TargetUrl = dto.TargetUrl;
                
                client.Updateable(ad).ExecuteCommand();
                _eventPublisher.EntityUpdated(ad);
                
                return Ok();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeleteAd(DeleteAdDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var ad = client.Queryable<Ad>().InSingle(dto.Id);
                if (ad == null)
                {
                    return Error("找不到该条信息");
                }

                client.Deleteable<Ad>(dto.Id).ExecuteCommand();
                _eventPublisher.EntityDeleted(ad);
                
                return Ok();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult BulkDeleteAds(BulkDeleteAdsDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                client.Deleteable<Ad>().In(dto.Ids).ExecuteCommand();
                _eventPublisher.EntitiesDeleted<Ad>(dto.Ids);
                return Ok();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryAdItem>> PagedQueryAds(PagedQueryAdsDto dto) 
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<Ad>()
                    .Where(it => it.AdPlaceId == dto.AdPlaceId)
                    .WhereIF(!dto.Keyword.IsNullOrEmpty(), it => it.Title.Contains(dto.Keyword))
                    .Select(t => new QueryAdItem()
                    {
                        Id = t.Id,
                        AdPlaceId = t.AdPlaceId,
                        Title = t.Title,
                        ImagePath = t.ImagePath,
                        TargetUrl = t.TargetUrl,
                        Order = t.OrderIndex,
                        IsShow = t.IsShow,
                    })
                    .OrderBy("order_index ASC,id DESC")
                    .ToPageList(page, pageSize, ref total);
                return new PagedList<QueryAdItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetAdOrder(SetAdOrderDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var ad = client.Queryable<Ad>().Select(it => new {it.Id}).First(it => it.Id == dto.Id);
                if (ad == null)
                    return Error("找不到该条信息");

                client.Updateable<Ad>()
                    .SetColumns(it => new Ad { OrderIndex = dto.OrderIndex})
                    .Where(it => it.Id == dto.Id)
                    .ExecuteCommand();

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetAdIsShow(SetAdIsShowDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var ad = client.Queryable<Ad>().Select(it => new {it.Id}).First(it => it.Id == dto.Id);
                if (ad == null)
                    return Error("找不到该条信息");

                client.Updateable<Ad>()
                    .SetColumns(it => new Ad { IsShow = dto.IsShow })
                    .Where(it => it.Id == dto.Id)
                    .ExecuteCommand();

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetAdsByCodeInfo> GetAdsByCode(GetAdsByCodeDto dto) 
        {
            using (var client = DbFactory.CreateClient())
            {
                var list = client.Queryable<Ad, AdPlace>((ad, place) => new object[]
                    {
                        JoinType.Left, ad.AdPlaceId == place.Id
                    })
                    .Where((ad, place) =>
                        ad.IsShow && place.Code.Equals(dto.Code, StringComparison.OrdinalIgnoreCase))
                    .Select((ad, place) => new
                    {
                        Id = ad.Id,
                        Title = ad.Title,
                        ImagePath = ad.ImagePath,
                        TargetUrl = ad.TargetUrl,
                    })
                    .OrderBy("order_index ASC,id DESC")
                    .ToList();
                return new GetAdsByCodeInfo()
                {
                    Ads = list.Select(it => new GetAdsByCodeItem
                    {
                        Id = it.Id,
                        Title = it.Title,
                        ImagePath = it.ImagePath.GetFullPath(),
                        TargetUrl = it.TargetUrl
                    }).ToList()
                };
            }
        }

        #endregion

        #region 广告位置

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetAdPlaceDetailInfo> GetAdPlaceDetail(GetAdPlaceDetailDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(dto.Id);
                if (adPlace == null)
                {
                    return Error("找不到此信息");
                }

                var resp = new GetAdPlaceDetailInfo
                {
                    Id = adPlace.Id,
                    Title = adPlace.Title,
                    Code = adPlace.Code,
                    Desc = adPlace.Desc,
                    AddTime = adPlace.Addtime
                };
                return resp;
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreateAdPlace(CreateAdPlaceDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var adPlace = new AdPlace
                {
                    Title = dto.Title,
                    Code = dto.Code,
                    Desc = dto.Desc,
                    Addtime = DateTime.Now
                };
                adPlace.Id = client.Insertable(adPlace).ExecuteReturnBigIdentity();

                _eventPublisher.EntityCreated(adPlace);
                return Ok();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateAdPlace(UpdateAdPlaceDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(dto.Id);
                if (adPlace == null)
                {
                    throw new AlertException("找不到该条信息");
                }

                adPlace.Title = dto.Title;
                adPlace.Code = dto.Code;
                adPlace.Desc = dto.Desc;

                client.Updateable(adPlace).ExecuteCommand();
                _eventPublisher.EntityUpdated(adPlace);
                
                return Ok();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeleteAdPlace(DeleteAdPlaceDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(dto.Id);
                if (adPlace == null)
                {
                    return Error("找不到该条信息");
                }

                client.Deleteable<AdPlace>(dto.Id).ExecuteCommand();
                _eventPublisher.EntityDeleted(adPlace);
                
                return Ok();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult BulkDeleteAdPlaces(BulkDeleteAdPlacesDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                client.Deleteable<AdPlace>().In(dto.Ids).ExecuteCommand();
                _eventPublisher.EntitiesDeleted<AdPlace>(dto.Ids);
                return Ok();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryAdPlaceItem>> PagedQueryAdPlaces(PagedQueryAdPlaceDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<AdPlace>()
                    .WhereIF(!dto.Keyword.IsNullOrEmpty(), t => t.Title.Contains(dto.Keyword))
                    .Select(t => new QueryAdPlaceItem()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Code = t.Code,
                        Desc = t.Desc,
                        AddTime = t.Addtime,
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize, ref total);
                return new PagedList<QueryAdPlaceItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        #endregion
    }
}