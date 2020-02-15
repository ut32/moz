using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.AdPlaces;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.AdPlaces;
using Moz.Bus.Services.Ads;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.adPlace")]
    public class AdPlaceController : AdminAuthBaseController
    {
        private readonly IAdService _adService;
        public AdPlaceController(IAdService adService)
        {
            this._adService = adService;
        }

        [AdminAuth(Permissions = "admin.adPlace.index")]
        public IActionResult Index()
        {
            var model = new IndexModel();
            return View("~/Administration/Views/AdPlace/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.adPlace.index")]
        public IActionResult PagedList(PagedQueryAdPlaceDto dto)
        {
            var list = _adService.PagedQueryAdPlaces(dto);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.Data.TotalCount,
                Data = list.Data.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.adPlace.create")]
        public IActionResult Create()
        {
            var model = new CreateModel();
            return View("~/Administration/Views/AdPlace/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.adPlace.create")]
        public IActionResult Create(CreateAdPlaceDto dto)
        {
            var result = _adService.CreateAdPlace(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.adPlace.update")]
        public IActionResult Update(GetAdPlaceDetailDto dto)
        {
            var getAdPlaceResult = _adService.GetAdPlaceDetail(dto);
            if (getAdPlaceResult.Code > 0)
            {
                return Json(getAdPlaceResult);
            }
            var model = new UpdateModel()
            {
                AdPlace = getAdPlaceResult.Data
            };
            return View("~/Administration/Views/AdPlace/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.adPlace.update")]
        public IActionResult Update(UpdateAdPlaceDto dto)
        {
            var updateResult = _adService.UpdateAdPlace(dto); 
            return Json(updateResult);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.adPlace.delete")]
        public IActionResult Delete(DeleteAdPlaceDto dto)
        {
            var deleteResult = _adService.DeleteAdPlace(dto);
            return Json(deleteResult); 
        }
    }
}
