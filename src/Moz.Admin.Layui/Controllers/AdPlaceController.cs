using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Bus.Dtos.AdPlaces;
using Moz.Bus.Services.Ads;
using Moz.Exceptions;

namespace Moz.Admin.Layui.Controllers
{
    //[AdminAuthorize(Permissions = "admin.adPlace")]
    public class AdPlaceController : AdminAuthBaseController
    {
        private readonly IAdService _adService;
        public AdPlaceController(IAdService adService)
        {
            this._adService = adService;
        }

        //[AdminAuthorize(Permissions = "admin.adPlace.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.AdPlaces.IndexModel();
            return View("~/Administration/Views/AdPlace/Index.cshtml",model);
        }
        
        //[AdminAuthorize(Permissions = "admin.adPlace.index")]
        public IActionResult PagedList(PagedQueryAdPlaceRequest request)
        {
            var list = _adService.PagedQueryAdPlaces(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        //[AdminAuthorize(Permissions = "admin.adPlace.create")]
        public IActionResult Create()
        {
            var model = new  Moz.Administration.Models.AdPlaces.CreateModel();
            return View("~/Administration/Views/AdPlace/Create.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.adPlace.create")]
        public IActionResult Create(Moz.Biz.Dtos.AdPlaces.CreateAdPlaceRequest request)
        {
            var resp = _adService.CreateAdPlace(request);
            return RespJson(resp);
        }
        
        //[AdminAuthorize(Permissions = "admin.adPlace.update")]
        public IActionResult Update(Moz.Biz.Dtos.AdPlaces.GetAdPlaceDetailRequest request)
        {
            var adPlace = _adService.GetAdPlaceDetail(request);
            if (adPlace == null)
            {
                throw new AlertException("信息不存在，可能被删除");
            }
            var model = new  Moz.Administration.Models.AdPlaces.UpdateModel()
            {
                AdPlace = adPlace
            };
            return View("~/Administration/Views/AdPlace/Update.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.adPlace.update")]
        public IActionResult Update(Moz.Biz.Dtos.AdPlaces.UpdateAdPlaceRequest request)
        {
            var resp = _adService.UpdateAdPlace(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.adPlace.delete")]
        public IActionResult Delete(Moz.Biz.Dtos.AdPlaces.DeleteAdPlaceRequest request)
        {
            var resp = _adService.DeleteAdPlace(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.adPlace.setisactive")]
        public IActionResult SetIsActive()
        {
            return RespJson(null);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.adPlace.setorderindex")]
        public IActionResult SetOrderIndex()
        {
            return RespJson(null);
        }
    }
}
