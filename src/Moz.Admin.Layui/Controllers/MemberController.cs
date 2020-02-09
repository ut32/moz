using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.Members;
using Moz.Administration.Models.Members;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Dtos.Roles;
using Moz.Bus.Services.Members;
using Moz.Bus.Services.Roles;
using Moz.Configuration;
using Moz.Core;
using Moz.Exceptions;
using Moz.Presentation.Administration.Models.Articles;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.member")]
    public class MemberController : AdminAuthBaseController
    {
        private readonly IMemberService _memberService;
        private readonly AdminSettings _adminSettings;
        private readonly IWorkContext _workContext;
        private readonly IRoleService _roleService;
        
        public MemberController(IMemberService memberService,AdminSettings adminSettings,IWorkContext workContext, IRoleService roleService)
        {
            _adminSettings = adminSettings;
            _memberService = memberService;
            _workContext = workContext;
            _roleService = roleService;
        }

        [AdminAuth(Permissions = "admin.member.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.Members.IndexModel();
            return View("~/Administration/Views/Member/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.member.index")]
        public IActionResult PagedList(PagedQueryMemberDto dto)
        {
            var pagedQueryMembersResult = _memberService.PagedQueryMembers(dto);
            if (pagedQueryMembersResult.Code > 0)
            {
                return Json(pagedQueryMembersResult);
            }
            var result = new
            {
                Code = 0,
                Message = "",
                Total = pagedQueryMembersResult.Data.TotalCount,
                Data = pagedQueryMembersResult.Data.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.member.create")]
        public IActionResult Create()
        {
            var model = new  Moz.Administration.Models.Members.CreateModel();
            return View("~/Administration/Views/Member/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.member.create")]
        public IActionResult Create(CreateMemberDto dto)
        {
            var result = _memberService.CreateMember(dto);
            return Json(result);
        }
        
        
        [AdminAuth(Permissions = "admin.member.update")]
        public IActionResult Update(GetMemberDetailDto dto)
        {
            var getMemberDetailResult = _memberService.GetMemberDetail(dto);
            if(getMemberDetailResult.Code>0)
            {
                return Json(getMemberDetailResult);
            }

            var pagedQueryRolesResult = _roleService.PagedQueryRoles(new PagedQueryRoleDto
            {
                PageSize = 100
            });
            if (pagedQueryRolesResult.Code > 0)
            {
                return Json(pagedQueryRolesResult);
            }

            var model = new UpdateModel
            {
                Member = getMemberDetailResult.Data,
                Roles = pagedQueryRolesResult.Data.List
            };
            return View("~/Administration/Views/Member/Update.cshtml", model);
        }


        [HttpPost]
        [AdminAuth(Permissions = "admin.member.update")]
        public IActionResult Update(UpdateMemberDto dto)
        {
            var result = _memberService.UpdateMember(dto); 
            return Json(result);
        } 

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.member.setisactive")]
        public IActionResult SetIsActive()
        {
            return RespJson(null);
        }
        
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public IActionResult ChangePwd()
        {
            return View("~/Administration/Views/Member/ChangePwd.cshtml");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangePwd(ChangePasswordDto dto)
        {
            dto.MemberId = _workContext.CurrentMember.Id;
            var result = _memberService.ChangePassword(dto);
            return Json(result);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ResetPwd(long[] ids)
        {
            var newPwd = "66669999";
            if (!_adminSettings.ResetPassword.IsNullOrEmpty())
                newPwd = _adminSettings.ResetPassword;
            
            var dto = _memberService.ResetPassword(new ResetPasswordDto()
            {
                NewPassword =newPwd,
                MemberIds = ids
            });
            return Json(dto); 
        }
    }
}