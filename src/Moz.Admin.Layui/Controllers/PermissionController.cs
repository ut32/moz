using System;
using System.Collections.Generic;
using System.Linq;
using Moz.Domain.Dtos.Members.Permissions;
using Moz.Domain.Services.Members;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.Members.Permissions;
using Moz.Bus.Services.Members;
using Moz.Exceptions;

namespace Moz.Administration.Controllers
{
    [AdminAuth(Permissions = "admin.permission")]
    public class PermissionController : AdminAuthBaseController
    {
        private readonly IMemberService _memberService;
        public PermissionController(IMemberService memberService)
        {
            this._memberService = memberService;
        }

       
        [AdminAuth(Permissions = "admin.permission.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.Permissions.IndexModel();
            return View("~/Administration/Views/Permission/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.permission.index")]
        public IActionResult PagedList(PagedQueryPermissionRequest request)
        {
            var list = _memberService.PagedQueryPermissions(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.permission.create")]
        public IActionResult Create()
        {
            var model = new  Moz.Administration.Models.Permissions.CreateModel();
            return View("~/Administration/Views/Permission/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.create")]
        public IActionResult Create(Moz.Domain.Dtos.Members.Permissions.CreatePermissionRequest request)
        {
            var resp = _memberService.CreatePermission(request);
            return RespJson(resp);
        }
        
        [AdminAuth(Permissions = "admin.permission.update")]
        public IActionResult Update(Moz.Domain.Dtos.Members.Permissions.GetPermissionDetailRequest request)
        {
            var permission = _memberService.GetPermissionDetail(request);
            if (permission == null)
            {
                throw new MozException("信息不存在，可能被删除");
            }
            var model = new  Moz.Administration.Models.Permissions.UpdateModel()
            {
                Permission = permission
            };
            return View("~/Administration/Views/Permission/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.update")]
        public IActionResult Update(Moz.Domain.Dtos.Members.Permissions.UpdatePermissionRequest request)
        {
            var resp = _memberService.UpdatePermission(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.delete")]
        public IActionResult Delete(Moz.Domain.Dtos.Members.Permissions.DeletePermissionRequest request)
        {
            var resp = _memberService.DeletePermission(request);
            return RespJson(resp);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.permission.isactive")]
        public IActionResult SetIsActive(SetPermissionIsActiveRequest request)
        {
            var resp = _memberService.SetPermissionIsActive(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        public IActionResult SetOrderIndex(SetPermissionOrderIndexRequest request)
        {
            var resp = _memberService.SetPermissionOrderIndex(request);
            return RespJson(resp);
        }
    }
}
