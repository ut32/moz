using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.AdminMenus;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Bus.Services.AdminMenus;
using Moz.Exceptions;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.menu")]
    public class MenuController : AdminAuthBaseController
    {
        private readonly IAdminMenuService _adminMenuService;
        public MenuController(IAdminMenuService adminMenuService)
        {
            this._adminMenuService = adminMenuService;
        }
        
        [AdminAuth(Permissions = "admin.menu.index")]
        public IActionResult Index()
        {
            return View("~/Administration/Views/Menu/Index.cshtml");
        }

        [AdminAuth(Permissions = "admin.menu.index")]
        public IActionResult PagedList(PagedQueryAdminMenusDto dto)
        {
            var pagedQueryAdminMenusResult = _adminMenuService.PagedQueryAdminMenus(dto);
            if (pagedQueryAdminMenusResult.Code > 0)
            {
                return Json(pagedQueryAdminMenusResult);
            }
            var result = new
            {
                Code = 0,
                Message = "",
                Total = pagedQueryAdminMenusResult.Data.TotalCount,
                Data = pagedQueryAdminMenusResult.Data.List
            };
            return Json(result);
        }

        
        [AdminAuth(Permissions = "admin.menu.create")]
        public IActionResult Create()
        {
            return View("~/Administration/Views/Menu/Create.cshtml");
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.menu.create")]
        public IActionResult Create(CreateAdminMenuDto dto)
        {
            var result = _adminMenuService.CreateAdminMenu(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.menu.update")]
        public IActionResult Update(GetAdminMenuDetailDto dto)
        { 
            var getAdminMenuDetailResult = _adminMenuService.GetAdminMenuDetail(dto);
            if (getAdminMenuDetailResult.Code>0)
            {
                return Json(getAdminMenuDetailResult);
            }

            var model = new UpdateModel()
            {
                Detail = getAdminMenuDetailResult.Data
            };
            return View("~/Administration/Views/Menu/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.menu.update")]
        public IActionResult Update(UpdateAdminMenuDto dto)
        {
            var result = _adminMenuService.UpdateAdminMenu(dto); 
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.menu.delete")]
        public IActionResult Delete(DeleteAdminMenuDto dto)
        {
            var result = _adminMenuService.DeleteAdminMenu(dto);
            return Json(result);
        }
        
        [HttpGet]
        public IActionResult AllSubMenus() 
        {
            var result = _adminMenuService.QueryChildrenByParentId(new QueryChildrenByParentIdDto());
            return Json(result.Data.AllSubs);
        }

        [HttpPost]
        public IActionResult SetOrderIndex(SetAdminMenuOrderIndexDto dto)
        {
            var result = _adminMenuService.SetAdminMenuOrderIndex(dto);
            return RespJson(result);
        }
    }
}