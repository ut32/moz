using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models.AdminMenus;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Domain.Services.AdminMenus;
using Moz.Exceptions;

namespace Moz.Administration.Controllers
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
        public IActionResult PagedList(PagedQueryAdminMenuRequest request)
        {
            var list = _adminMenuService.PagedQueryAdminMenus(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
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
        public IActionResult Create(CreateAdminMenuRequest request)
        {
            var resp = _adminMenuService.CreateAdminMenu(request);
            return RespJson(resp);
        }
        
        [AdminAuth(Permissions = "admin.menu.update")]
        public IActionResult Update(GetAdminMenuDetailRequest request)
        {
            var adminMenu = _adminMenuService.GetAdminMenuDetail(request);
            if (adminMenu == null)
            {
                throw new MozException("信息不存在，可能被删除");
            }

            var model = new UpdateModel()
            {
                Detail = adminMenu
            };
            return View("~/Administration/Views/Menu/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.menu.update")]
        public IActionResult Update(UpdateAdminMenuRequest request)
        {
            var resp = _adminMenuService.UpdateAdminMenu(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.menu.delete")]
        public IActionResult Delete(DeleteAdminMenuRequest request)
        {
            var resp = _adminMenuService.DeleteAdminMenu(request);
            return RespJson(resp);
        }
        
        [HttpGet]
        public IActionResult AllSubMenus() 
        {
            var resp = _adminMenuService.QueryChildrenByParentId(new QueryChildrenByParentIdRequest());
            return Json(resp.AllSubs);
        }

        [HttpPost]
        public IActionResult SetOrderIndex(SetAdminMenuOrderIndexRequest request)
        {
            var resp = _adminMenuService.SetAdminMenuOrderIndex(request);
            return RespJson(resp);
        }
        
        
    }
    
}