using System;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.Ads;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.Ads;
using Moz.Bus.Services.Ads;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.ad")]
    public class AdController : AdminAuthBaseController
    {
        private readonly IAdService _adService;
        public AdController(IAdService adService)
        {
            this._adService = adService;
        }

        [AdminAuth(Permissions = "admin.ad.index")]
        public IActionResult Index(long adPlaceId) 
        {
            var model = new IndexModel
            {
                AdPlaceId = adPlaceId
            };
            return View("~/Administration/Views/Ad/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.ad.index")]
        public IActionResult PagedList(PagedQueryAdsDto dto)
        {
            var list = _adService.PagedQueryAds(dto);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.Data.TotalCount,
                Data = list.Data.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.ad.create")]
        public IActionResult Create(long adPlaceId)
        {
            var model = new CreateModel
            {
                AdPlaceId = adPlaceId
            };
            return View("~/Administration/Views/Ad/Create.cshtml",model);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.ad.create")]
        public IActionResult Create(CreateAdDto dto)
        {
            var result = _adService.CreateAd(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.ad.update")]
        public IActionResult Update(GetAdDetailDto dto)
        {
            var getAdResult = _adService.GetAdDetail(dto);
            if (getAdResult.Code > 0)
            {
                return Json(getAdResult);
            } 
            var model = new UpdateModel()
            {
                Ad = getAdResult.Data
            };
            return View("~/Administration/Views/Ad/Update.cshtml",model);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.ad.update")]
        public IActionResult Update(UpdateAdDto dto)
        {
            var result = _adService.UpdateAd(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.ad.delete")]
        public IActionResult Delete(DeleteAdDto dto)
        {
            var result = _adService.DeleteAd(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.ad.setIsActive")]
        public IActionResult SetIsShow(SetAdIsShowDto dto)
        {
            var result = _adService.SetAdIsShow(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.ad.setOrderIndex")]
        public IActionResult SetOrderIndex(SetAdOrderDto dto)
        {
            var result = _adService.SetAdOrder(dto);
            return Json(result);
        }
    }
}
