using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.Permissions;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.Permissions;
using Moz.Bus.Services.Members;
using Moz.Bus.Services.Permissions;
using Moz.Exceptions;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.permission")]
    public class PermissionController : AdminAuthBaseController
    {
        private readonly IMemberService _memberService;
        private readonly IPermissionService _permissionService;
        public PermissionController(IMemberService memberService, IPermissionService permissionService)
        {
            this._memberService = memberService;
            _permissionService = permissionService;
        }

       
        [AdminAuth(Permissions = "admin.permission.index")]
        public IActionResult Index()
        {
            var model = new IndexModel();
            return View("~/Administration/Views/Permission/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.permission.index")]
        public IActionResult PagedList(PagedQueryPermissionDto dto)
        {
            var result = _permissionService.PagedQueryPermissions(dto);
            if (result.Code > 0)
            {
                return Json(result);
            }

            var data = new
            {
                Code = 0,
                Message = "",
                Total = result.Data.TotalCount,
                Data = result.Data.List
            };
            return Json(data);
        }
        
        [AdminAuth(Permissions = "admin.permission.create")]
        public IActionResult Create()
        {
            var model = new  CreateModel();
            return View("~/Administration/Views/Permission/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.create")]
        public IActionResult Create(CreatePermissionDto dto)
        {
            var result = _permissionService.CreatePermission(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.permission.update")]
        public IActionResult Update(GetPermissionDetailDto dto)
        {
            var result = _permissionService.GetPermissionDetail(dto);
            if (result.Code > 0)
            {
                return Json(result);
            }
            var model = new  UpdateModel()
            {
                Permission = result.Data
            };
            return View("~/Administration/Views/Permission/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.update")]
        public IActionResult Update(UpdatePermissionDto dto)
        {
            var result = _permissionService.UpdatePermission(dto); 
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.delete")]
        public IActionResult Delete(DeletePermissionDto dto)
        {
            var result = _permissionService.DeletePermission(dto);
            return Json(result);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.isActive")]
        public IActionResult SetIsActive(SetIsActivePermissionDto dto)
        {
            var result = _permissionService.SetIsActive(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.setOrderIndex")]
        public IActionResult SetOrderIndex(SetOrderIndexPermissionDto dto)
        {
            var result = _permissionService.SetOrderIndex(dto);
            return Json(result);
        }
        
        [HttpGet]
        public IActionResult AllSubPermissions(long? parentId)
        {
            var result = _permissionService.QuerySubPermissionsByParentId(parentId);
            return Json(result.Data);
        }
    }
}
