using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Moz.Administration.Common;
using Moz.Administration.Models.AdminMenus;
using Moz.Administration.Models.Roles;
using Moz.Domain.Dtos.AdminMenus;
using Moz.Domain.Dtos.Members.Permissions;
using Moz.Domain.Dtos.Members.Roles;
using Moz.Domain.Services.AdminMenus;
using Moz.Domain.Services.Members;
using Moz.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.Members.Roles;
using Moz.Bus.Services.Members;
using Moz.Core;

namespace Moz.Administration.Controllers
{
    [AdminAuth(Permissions = "admin.role")]
    public class RoleController : AdminAuthBaseController
    {
        private readonly IMemberService _memberService;
        private readonly IAdminMenuService _adminMenuService;
        private readonly IWorkContext _workContext;
        public RoleController(IMemberService memberService,IAdminMenuService adminMenuService,IWorkContext workContext)
        {
            this._memberService = memberService;
            this._adminMenuService = adminMenuService;
            this._workContext = workContext;
        }

        [AdminAuth(Permissions = "admin.role.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.Roles.IndexModel();
            return View("~/Administration/Views/Role/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.role.index")]
        public IActionResult PagedList(PagedQueryRoleRequest request)
        {
            var list = _memberService.PagedQueryRoles(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.role.crate")]
        public IActionResult Create()
        {
            var model = new Moz.Administration.Models.Roles.CreateModel();
            return View("~/Administration/Views/Role/Create.cshtml",model);
        }
        

        [AdminAuth(Permissions = "admin.role.crate")]
        [HttpPost]
        public IActionResult Create(Moz.Domain.Dtos.Members.Roles.CreateRoleRequest request)
        {
            var resp = _memberService.CreateRole(request);
            return RespJson(resp);
        }
        
        [AdminAuth(Permissions = "admin.role.update")]
        public IActionResult Update(Moz.Domain.Dtos.Members.Roles.GetRoleDetailRequest request)
        {
            var role = _memberService.GetRoleDetail(request);
            if (role == null)
            {
                throw new MozException("信息不存在，可能被删除");
            }
            var model = new Moz.Administration.Models.Roles.UpdateModel()
            {
                Role = role
            };
            return View("~/Administration/Views/Role/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.update")]
        public IActionResult Update(Moz.Domain.Dtos.Members.Roles.UpdateRoleRequest request)
        {
            var resp = _memberService.UpdateRole(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.role.delete")]
        public IActionResult Delete(Moz.Domain.Dtos.Members.Roles.DeleteRoleRequest request)
        {
            var resp = _memberService.DeleteRole(request);
            return RespJson(resp);
        }

        [AdminAuth(Permissions = "admin.role.configPermission")]
        public IActionResult ConfigPermission(GetPermissionsByRoleRequest request)
        {

            var allPermissions = _memberService.PagedQueryPermissions(new PagedQueryPermissionRequest()
            {
                Page = 1,
                PageSize = 10000
            });
            var permissionsByRole = _memberService.GetPermissionsByRole(request);

            var configPermissions = allPermissions.List
                .OrderBy(t => t.OrderIndex)
                .ThenBy(t => t.Id)
                .Select(t => new ConfigPermissionItem()
                {
                    id = t.Id,
                    name = t.Name,
                    pId = t.ParentId ?? 0,
                    open = t.ParentId == null,
                    @checked = permissionsByRole.Permissions.Any(x => x.Id == t.Id)
                }).ToList();
            var model = new ConfigPermissionModel()
            {
                RoleId = request.RoleId,
                Permissions = configPermissions
            };

            return View("~/Administration/Views/Role/ConfigPermission.cshtml", model);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.configPermission")]
        public IActionResult ConfigPermission(Moz.Domain.Dtos.Members.Roles.ConfigPermissionRequest request)
        {
            var resp = _memberService.ConfigPermission(request); 
            //_workContext.CurrentMember
            return RespJson(resp);
        }
        
        [AdminAuth(Permissions = "admin.role.configMenu")]
        public IActionResult ConfigMenu(GetMenusByRoleRequest request)
        {

            var allMenus = _adminMenuService.PagedQueryAdminMenus(new PagedQueryAdminMenuRequest()
            {
                Page = 1,
                PageSize = 10000
            });
            var menusByRole = _adminMenuService.GetMenusByRole(request);

            var configMenus = allMenus.List
                .OrderBy(t => t.OrderIndex)
                .ThenBy(t => t.Id)
                .Select(t => new ConfigMenuItem()
                {
                    id = t.Id,
                    name = t.Name,
                    pId = t.ParentId ?? 0,
                    open = t.ParentId == null,
                    @checked = menusByRole.Menus.Any(x => x.Id == t.Id)
                }).ToList();
            var model = new ConfigMenuModel()
            {
                RoleId = request.RoleId,
                Menus = configMenus
            };

            return View("~/Administration/Views/Role/ConfigMenu.cshtml", model);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.configMenu")]
        public IActionResult ConfigMenu(ConfigMenuRequest request)
        {
            var resp = _memberService.ConfigMenu(request); 
            return RespJson(resp);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.setisactive")]
        public IActionResult SetIsActive(SetRoleIsActiveRequest request)
        {
            var resp = _memberService.SetRoleIsActive(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.role.setisadmin")]
        public IActionResult SetIsAdmin(SetRoleIsAdminRequest request)
        {
            var resp = _memberService.SetRoleIsAdmin(request);
            return RespJson(resp);
        }
    }
}