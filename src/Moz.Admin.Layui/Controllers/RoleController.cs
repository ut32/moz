using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.Roles;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Bus.Dtos.Permissions;
using Moz.Bus.Dtos.Roles;
using Moz.Bus.Services.AdminMenus;
using Moz.Bus.Services.Members;
using Moz.Bus.Services.Permissions;
using Moz.Bus.Services.Roles;
using Moz.Core;
using Moz.Exceptions;
using GetMenusByRoleDto = Moz.Bus.Dtos.Roles.GetMenusByRoleDto;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.role")]
    public class RoleController : AdminAuthBaseController
    {
        private readonly IMemberService _memberService;
        private readonly IAdminMenuService _adminMenuService;
        private readonly IWorkContext _workContext;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        public RoleController(IMemberService memberService,IAdminMenuService adminMenuService,IWorkContext workContext, IRoleService roleService, IPermissionService permissionService)
        {
            this._memberService = memberService;
            this._adminMenuService = adminMenuService;
            this._workContext = workContext;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        [AdminAuth(Permissions = "admin.role.index")]
        public IActionResult Index()
        {
            var model = new IndexModel();
            return View("~/Administration/Views/Role/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.role.index")]
        public IActionResult PagedList(PagedQueryRoleDto dto)
        {
            var list = _roleService.PagedQueryRoles(dto);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.Data.TotalCount,
                Data = list.Data.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.role.crate")]
        public IActionResult Create()
        {
            var model = new CreateModel();
            return View("~/Administration/Views/Role/Create.cshtml",model);
        }


        [AdminAuth(Permissions = "admin.role.crate")]
        [HttpPost]
        public IActionResult Create(CreateRoleDto dto)
        {
            var result = _roleService.CreateRole(dto);
            return Json(result);
        }

        [AdminAuth(Permissions = "admin.role.update")]
        public IActionResult Update(GetRoleDetailDto dto)
        {
            var result = _roleService.GetRoleDetail(dto);
            if (result.Code>0)
            {
                return Json(result);
            }
            var model = new UpdateModel()
            {
                Role = result.Data
            };
            return View("~/Administration/Views/Role/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.update")]
        public IActionResult Update(UpdateRoleDto dto)
        {
            var result = _roleService.UpdateRole(dto); 
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.role.delete")]
        public IActionResult Delete(DeleteRoleDto dto)
        {
            var result = _roleService.DeleteRole(dto);
            return Json(result);
        }
        
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.role.setIsActive")]
        public IActionResult SetIsActive(SetIsActiveRoleDto dto)
        {
            var result = _roleService.SetIsActive(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.role.setIsAdmin")]
        public IActionResult SetIsAdmin(SetIsAdminRoleDto dto)
        {
            var result = _roleService.SetIsAdmin(dto);
            return Json(result);
        }

        [AdminAuth(Permissions = "admin.role.configPermission")]
        public IActionResult ConfigPermission(GetPermissionsByRoleDto dto)
        {

            var pagedQueryPermissionsResult 
                = _permissionService.PagedQueryPermissions(new PagedQueryPermissionDto()
            {
                Page = 1,
                PageSize = 10000
            });
            if (pagedQueryPermissionsResult.Code > 0)
            {
                return Json(pagedQueryPermissionsResult);
            }

            var getPermissionsResult = _roleService.GetPermissionsByRole(dto);
            if (getPermissionsResult.Code > 0)
            {
                return Json(getPermissionsResult);
            }

            var permissionsByRole = getPermissionsResult.Data;
            
            var configPermissions = pagedQueryPermissionsResult.Data.List
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
                RoleId = dto.RoleId,
                Permissions = configPermissions
            };

            return View("~/Administration/Views/Role/ConfigPermission.cshtml", model);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.configPermission")]
        public IActionResult ConfigPermission(ConfigPermissionDto dto)
        {
            var result = _roleService.ConfigPermission(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.role.configMenu")]
        public IActionResult ConfigMenu(GetMenusByRoleDto dto)
        {

            var allMenus = _adminMenuService.PagedQueryAdminMenus(new PagedQueryAdminMenusDto()
            {
                Page = 1,
                PageSize = 10000
            });
            
            var getMenusByRoleResult = _roleService.GetMenusByRole(dto);
            if (getMenusByRoleResult.Code > 0)
            {
                return Json(getMenusByRoleResult);
            }

            var menusByRole = getMenusByRoleResult.Data;

            var configMenus = allMenus.Data.List
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
                RoleId = dto.RoleId,
                Menus = configMenus
            };

            return View("~/Administration/Views/Role/ConfigMenu.cshtml", model);
        }

        [HttpPost]
        [AdminAuth(Permissions = "admin.role.configMenu")]
        public IActionResult ConfigMenu(ConfigMenuDto dto)
        {
            var result = _roleService.ConfigMenu(dto); 
            return Json(result);
        }

        
    }
}