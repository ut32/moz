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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetAdDetailApo> GetAdDetail(ServRequest<GetAdDetailDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().InSingle(request.Data.Id);
                if (ad == null)
                {
                    return Error("找不到此信息");
                }

                var resp = new GetAdDetailApo();
                resp.Id = ad.Id;
                resp.AdPlaceId = ad.AdPlaceId;
                resp.Title = ad.Title;
                resp.ImagePath = ad.ImagePath;
                resp.TargetUrl = ad.TargetUrl;
                resp.Order = ad.OrderIndex;
                resp.IsShow = ad.IsShow;
                return resp;
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult CreateAd(ServRequest<CreateAdDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = new Ad
                {
                    AdPlaceId = request.Data.AdPlaceId,
                    Title = request.Data.Title,
                    ImagePath = request.Data.ImagePath,
                    TargetUrl = request.Data.TargetUrl,
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult UpdateAd(ServRequest<UpdateAdDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().InSingle(request.Data.Id);
                if (ad == null)
                {
                    return Error("找不到该条信息");
                }

                //ad.AdPlaceId = request.AdPlaceId;
                ad.Title = request.Data.Title;
                ad.ImagePath = request.Data.ImagePath;
                ad.TargetUrl = request.Data.TargetUrl;
                ////ad.Order = request.Order;
                //ad.IsShow = request.IsShow;
                client.Updateable(ad).ExecuteCommand();
                _eventPublisher.EntityUpdated(ad);
                return Ok();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult DeleteAd(ServRequest<DeleteAdDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().InSingle(request.Data.Id);
                if (ad == null)
                {
                    return Error("找不到该条信息");
                }

                client.Deleteable<Ad>(request.Data.Id).ExecuteCommand();
                _eventPublisher.EntityDeleted(ad);
                return Ok();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult BulkDeleteAds(ServRequest<BulkDeleteAdsDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<Ad>().In(request.Data.Ids).ExecuteCommand();
                _eventPublisher.EntitiesDeleted<Ad>(request.Data.Ids);
                return Ok();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<PagedList<QueryAdItem>> PagedQueryAds(ServRequest<PagedQueryAdsDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Ad>()
                    .Where(it => it.AdPlaceId == request.Data.AdPlaceId)
                    .WhereIF(!request.Data.Keyword.IsNullOrEmpty(), it => it.Title.Contains(request.Data.Keyword))
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult SetAdOrder(ServRequest<SetAdOrderDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().Select(it => new {it.Id}).First(it => it.Id == request.Data.Id);
                if (ad == null)
                    return Error("找不到该条信息");

                client.Updateable<Ad>()
                    .SetColumns(it => new Ad {OrderIndex = request.Data.OrderIndex})
                    .Where(it => it.Id == request.Data.Id)
                    .ExecuteCommand();

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ServResult SetAdIsShow(ServRequest<SetAdIsShowDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().Select(it => new {it.Id}).First(it => it.Id == request.Data.Id);
                if (ad == null)
                    return Error("找不到该条信息");

                client.Updateable<Ad>()
                    .SetColumns(it => new Ad {IsShow = request.Data.IsShow})
                    .Where(it => it.Id == request.Data.Id)
                    .ExecuteCommand();

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetAdsByCodeApo> GetAdsByCode(ServRequest<GetAdsByCodeDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<Ad, AdPlace>((ad, place) => new object[]
                    {
                        JoinType.Left, ad.AdPlaceId == place.Id
                    })
                    .Where((ad, place) =>
                        ad.IsShow && place.Code.Equals(request.Data.Code, StringComparison.OrdinalIgnoreCase))
                    .Select((ad, place) => new
                    {
                        Id = ad.Id,
                        Title = ad.Title,
                        ImagePath = ad.ImagePath,
                        TargetUrl = ad.TargetUrl,
                    })
                    .OrderBy("order_index ASC,id DESC")
                    .ToList();
                return new GetAdsByCodeApo()
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetAdPlaceDetailApo> GetAdPlaceDetail(ServRequest<GetAdPlaceDetailDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(request.Data.Id);
                if (adPlace == null)
                {
                    return Error("找不到此信息");
                }

                var resp = new GetAdPlaceDetailApo
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult CreateAdPlace(ServRequest<CreateAdPlaceDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = new AdPlace
                {
                    Title = request.Data.Title,
                    Code = request.Data.Code,
                    Desc = request.Data.Desc,
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult UpdateAdPlace(ServRequest<UpdateAdPlaceDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(request.Data.Id);
                if (adPlace == null)
                {
                    throw new AlertException("找不到该条信息");
                }

                adPlace.Title = request.Data.Title;
                adPlace.Code = request.Data.Code;
                adPlace.Desc = request.Data.Desc;

                client.Updateable(adPlace).ExecuteCommand();
                _eventPublisher.EntityUpdated(adPlace);
                return Ok();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult DeleteAdPlace(ServRequest<DeleteAdPlaceDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(request.Data.Id);
                if (adPlace == null)
                {
                    return Error("找不到该条信息");
                }

                client.Deleteable<AdPlace>(request.Data.Id).ExecuteCommand();
                _eventPublisher.EntityDeleted(adPlace);
                return Ok();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult BulkDeleteAdPlaces(ServRequest<BulkDeleteAdPlacesDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<AdPlace>().In(request.Data.Ids).ExecuteCommand();
                _eventPublisher.EntitiesDeleted<AdPlace>(request.Data.Ids);
                return Ok();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<PagedList<QueryAdPlaceItem>> PagedQueryAdPlaces(ServRequest<PagedQueryAdPlaceDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<AdPlace>()
                    .WhereIF(!request.Data.Keyword.IsNullOrEmpty(), t => t.Title.Contains(request.Data.Keyword))
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