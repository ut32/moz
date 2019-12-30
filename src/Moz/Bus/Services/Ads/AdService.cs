using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Biz.Dtos.AdPlaces;
using Moz.Biz.Dtos.Ads;
using Moz.Bus.Dtos.Ads;
using Moz.CMS.Model.Ad;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;
using SqlSugar;

namespace Moz.Bus.Services.Ads
{
    public partial class AdService : IAdService
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
        public GetAdDetailResponse GetAdDetail(GetAdDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                 var ad = client.Queryable<Ad>().InSingle(request.Id);
                 if(ad == null)
                 {
                    return null;
                 }
                 var resp = new GetAdDetailResponse();
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
        public CreateAdResponse CreateAd(CreateAdRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = new Ad
                {
                    AdPlaceId = request.AdPlaceId,
                    Title = request.Title,
                    ImagePath = request.ImagePath,
                    TargetUrl = request.TargetUrl,
                    OrderIndex = 0,
                    IsShow = true
                };
                ad.Id = client.Insertable(ad).ExecuteReturnBigIdentity();
                
                _eventPublisher.EntityCreated(ad);
                return new CreateAdResponse();
            }
        }
        
        

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpdateAdResponse UpdateAd(UpdateAdRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().InSingle(request.Id);
                if (ad == null)
                {
                    throw new MozException("找不到该条信息");
                }

                //ad.AdPlaceId = request.AdPlaceId;
                ad.Title = request.Title;
                ad.ImagePath = request.ImagePath;
                ad.TargetUrl = request.TargetUrl;
                ////ad.Order = request.Order;
                //ad.IsShow = request.IsShow;
                client.Updateable( ad).ExecuteCommand();    
                _eventPublisher.EntityUpdated(ad);
                return new UpdateAdResponse();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteAdResponse DeleteAd(DeleteAdRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().InSingle(request.Id);
                if (ad == null)
                {
                    throw new MozException("找不到该条信息");
                }

                client.Deleteable<Ad>(request.Id).ExecuteCommand();
                _eventPublisher.EntityDeleted(ad);
                return new DeleteAdResponse();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BulkDeleteAdsResponse BulkDeleteAds(BulkDeleteAdsRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<Ad>().In(request.Ids).ExecuteCommand();
                _eventPublisher.EntitiesDeleted<Ad>(request.Ids);         
                return new BulkDeleteAdsResponse();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryAdResponse PagedQueryAds(PagedQueryAdRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Ad>()
                    .Where(it=>it.AdPlaceId== request.AdPlaceId)
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), it=>it.Title.Contains(request.Keyword))
                    .Select(t=>new QueryAdItem()
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
                    .ToPageList(page, pageSize,ref total);
                return new PagedQueryAdResponse()
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
        public SetAdOrderResponse SetAdOrder(SetAdOrderRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().Select(it => new {it.Id}).First(it=>it.Id == request.Id);
                if (ad==null)
                    throw new MozException("找不到该条信息");

                client.Updateable<Ad>()
                    .UpdateColumns(it => new Ad {OrderIndex = request.OrderIndex})
                    .Where(it => it.Id == request.Id)
                    .ExecuteCommand();

                return new SetAdOrderResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public SetAdIsShowResponse SetAdIsShow(SetAdIsShowRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var ad = client.Queryable<Ad>().Select(it => new {it.Id}).First(it=>it.Id == request.Id);
                if (ad==null)
                    throw new MozException("找不到该条信息");

                client.Updateable<Ad>()
                    .UpdateColumns(it => new Ad { IsShow = request.IsShow })
                    .Where(it => it.Id == request.Id)
                    .ExecuteCommand();

                return new SetAdIsShowResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetAdsByCodeResponse GetAdsByCode(GetAdsByCodeRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<Ad, AdPlace>((ad, place) => new object[]
                    {
                        JoinType.Left, ad.AdPlaceId == place.Id
                    })
                    .Where((ad, place) => ad.IsShow && place.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase))
                    .Select((ad, place) => new
                    {
                        Id = ad.Id,
                        Title = ad.Title,
                        ImagePath = ad.ImagePath,
                        TargetUrl = ad.TargetUrl,
                    })
                    .OrderBy("order_index ASC,id DESC")
                    .ToList();
                return new GetAdsByCodeResponse()
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
        public GetAdPlaceDetailResponse GetAdPlaceDetail(GetAdPlaceDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                 var adPlace = client.Queryable<AdPlace>().InSingle(request.Id);
                 if(adPlace == null)
                 {
                    return null;
                 }
                 var resp = new GetAdPlaceDetailResponse();
                 resp.Id = adPlace.Id;
                 resp.Title = adPlace.Title;
                 resp.Code = adPlace.Code;
                 resp.Desc = adPlace.Desc;
                 resp.Addtime = adPlace.Addtime;
                 return resp;
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateAdPlaceResponse CreateAdPlace(CreateAdPlaceRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = new AdPlace
                {
                    Title = request.Title, Code = request.Code, Desc = request.Desc, Addtime = DateTime.Now
                };
                adPlace.Id = client.Insertable(adPlace).ExecuteReturnBigIdentity();
                
                _eventPublisher.EntityCreated(adPlace);
                return new CreateAdPlaceResponse();
            }
        }
        
        

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpdateAdPlaceResponse UpdateAdPlace(UpdateAdPlaceRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(request.Id);
                if (adPlace == null)
                {
                    throw new MozException("找不到该条信息");
                }

                adPlace.Title = request.Title;
                adPlace.Code = request.Code;
                adPlace.Desc = request.Desc;
                adPlace.Addtime = request.Addtime;
                client.Updateable( adPlace).ExecuteCommand();    
                _eventPublisher.EntityUpdated(adPlace);
                return new UpdateAdPlaceResponse();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteAdPlaceResponse DeleteAdPlace(DeleteAdPlaceRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adPlace = client.Queryable<AdPlace>().InSingle(request.Id);
                if (adPlace == null)
                {
                    throw new MozException("找不到该条信息");
                }

                client.Deleteable<AdPlace>(request.Id).ExecuteCommand();
                _eventPublisher.EntityDeleted(adPlace);
                return new DeleteAdPlaceResponse();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BulkDeleteAdPlacesResponse BulkDeleteAdPlaces(BulkDeleteAdPlacesRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<AdPlace>().In(request.Ids).ExecuteCommand();
                _eventPublisher.EntitiesDeleted<AdPlace>(request.Ids);         
                return new BulkDeleteAdPlacesResponse();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryAdPlaceResponse PagedQueryAdPlaces(PagedQueryAdPlaceRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<AdPlace>()
                    //.WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryAdPlaceItem()
                    {
                        Id = t.Id, 
                        Title = t.Title, 
                        Code = t.Code, 
                        Desc = t.Desc, 
                        Addtime = t.Addtime, 
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedQueryAdPlaceResponse()
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