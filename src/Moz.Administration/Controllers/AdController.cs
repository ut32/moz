using System;﻿
using Microsoft.AspNetCore.Mvc;
using Moz.Administration.Common;
using Moz.Biz.Dtos.Ads;
using Moz.Exceptions;
using Moz.Biz.Services.Ads;

namespace Moz.Administration.Controllers
{
    //[AdminAuthorize(Permissions = "admin.ad")]
    public class AdController : AdminAuthBaseController
    {
        private readonly IAdService _adService;
        public AdController(IAdService adService)
        {
            this._adService = adService;
        }

        //[AdminAuthorize(Permissions = "admin.ad.index")]
        public IActionResult Index(long adPlaceId) 
        {
            var model = new Models.Ads.IndexModel
            {
                AdPlaceId = adPlaceId
            };
            return View("~/Administration/Views/Ad/Index.cshtml",model);
        }
        
        //[AdminAuthorize(Permissions = "admin.ad.index")]
        public IActionResult PagedList(Moz.Biz.Dtos.Ads.PagedQueryAdRequest request)
        {
            var list = _adService.PagedQueryAds(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        //[AdminAuthorize(Permissions = "admin.ad.create")]
        public IActionResult Create(long adPlaceId)
        {
            var model = new Moz.Administration.Models.Ads.CreateModel
            {
                AdPlaceId = adPlaceId
            };
            return View("~/Administration/Views/Ad/Create.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.ad.create")]
        public IActionResult Create(Moz.Biz.Dtos.Ads.CreateAdRequest request)
        {
            var resp = _adService.CreateAd(request);
            return RespJson(resp);
        }
        
        //[AdminAuthorize(Permissions = "admin.ad.update")]
        public IActionResult Update(Moz.Biz.Dtos.Ads.GetAdDetailRequest request)
        {
            var ad = _adService.GetAdDetail(request);
            if (ad == null)
            {
                throw new MozException("信息不存在，可能被删除");
            }
            var model = new  Moz.Administration.Models.Ads.UpdateModel()
            {
                Ad = ad
            };
            return View("~/Administration/Views/Ad/Update.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.ad.update")]
        public IActionResult Update(Moz.Biz.Dtos.Ads.UpdateAdRequest request)
        {
            var resp = _adService.UpdateAd(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.ad.delete")]
        public IActionResult Delete(Moz.Biz.Dtos.Ads.DeleteAdRequest request)
        {
            var resp = _adService.DeleteAd(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.ad.setisactive")]
        public IActionResult SetIsShow(SetAdIsShowRequest request)
        {
            var resp = _adService.SetAdIsShow(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.ad.setorderindex")]
        public IActionResult SetOrderIndex(SetAdOrderRequest request)
        {
            var resp = _adService.SetAdOrder(request);
            return RespJson(resp);
        }
    }
}
