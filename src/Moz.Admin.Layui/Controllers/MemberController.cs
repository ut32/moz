using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Moz.Administration.Common;
using Moz.Administration.Models.Members;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Dtos.Members.Roles;
using Moz.Bus.Services.Members;
using Moz.Configuration;
using Moz.Core;
using Moz.Exceptions;
using Moz.Presentation.Administration.Models.Articles;

namespace Moz.Admin.Layui.Controllers
{
    //[Permission(Name = "MEMBER_MANAGE")]
    public class MemberController : AdminAuthBaseController
    {
        private readonly IMemberService _memberService;
        private readonly AdminSettings _adminSettings;
        private readonly IWorkContext _workContext;
        
        public MemberController(IMemberService memberService,AdminSettings adminSettings,IWorkContext workContext)
        {
            _adminSettings = adminSettings;
            _memberService = memberService;
            _workContext = workContext;
        }

        //[AdminAuthorize(Permissions = "admin.member.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.Members.IndexModel();
            return View("~/Administration/Views/Member/Index.cshtml",model);
        }
        
        //[AdminAuthorize(Permissions = "admin.member.index")]
        public IActionResult PagedList(PagedQueryMemberRequest request)
        {
            var list = _memberService.PagedQueryMembers(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        //[AdminAuthorize(Permissions = "admin.member.create")]
        public IActionResult Create()
        {
            var model = new  Moz.Administration.Models.Members.CreateModel();
            return View("~/Administration/Views/Member/Create.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.member.create")]
        public IActionResult Create(Moz.Biz.Dtos.Members.CreateMemberRequest request)
        {
            //var resp = _memberService.CreateMember(request);
            return RespJson(new{});
        }
        
        //[AdminAuthorize(Permissions = "admin.member.update")]
        public IActionResult Update(long id)
        {
            var request = new GetMemberDetailRequest
            {
                Id = id
            };
            var member = _memberService.GetMemberDetail(request);
            if (member == null)
                throw new MozException("信息不存在，可能被删除");

            var roles = _memberService.PagedQueryRoles(new PagedQueryRoleRequest
            {
                PageSize = 100
            });
            var model = new Moz.Administration.Models.Members.UpdateModel
            {
                Member = member,
                Roles = roles.List
            };
            return View("~/Administration/Views/Member/Update.cshtml", model);
        }


        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.member.update")]
        public IActionResult Update(SaveUpdateModel model)
        {
            var request = new UpdateMemberRequest
            {
                Id = model.Id,
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                Mobile = model.Mobile,
                Nickname = model.Nickname,
                Avatar = model.Avatar,
                Gender = model.Gender,
                BirthDay = model.BirthDay,
                Roles = model.Roles
            };
            var resp = _memberService.UpdateMember(request); 
            return RespJson(new{});
        }

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.member.setisactive")]
        public IActionResult SetIsActive()
        {
            return RespJson(null);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.member.setorderindex")]
        public IActionResult SetOrderIndex()
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangePwd(ChangePwdModel model)
        {
            var request = new ChangePasswordRequest
            {
                ConfirmPassword = model.ConfirmPassword,
                MemberId = _workContext.CurrentMember.Id,
                NewPassword = model.NewPassword,
                OldPassword = model.OldPassword
            };
            var resp = _memberService.ChangePassword(request);
            return RespJson(resp);
        }
        

        [HttpPost]
        public IActionResult ResetPwd(long[] ids)
        {
            var newPwd = "66669999";
            if (!_adminSettings.ResetPassword.IsNullOrEmpty())
                newPwd = _adminSettings.ResetPassword;
            
            var response = _memberService.ResetPassword(new ResetPasswordRequest()
            {
                NewPassword =newPwd,
                MemberIds = ids
            });
            return RespJson(response); 
        }

        #region 权限管理

        public IActionResult PermissionList()
        {
            /*
            var list = _memberService.GetPermissionsPagedList(
                t=>new Permission(){ Id=t.Id,Name = t.Name,Code = t.Code,IsActive = t.IsActive}, 
                null,
                null,
                (searchModel.Page ?? 1) - 1,
                searchModel.NumPerPage ?? 50); 
            var model = new PermissionListModel
            {
                PagedList = list
            };
            */

            //return View("~/Administration/Views/Member/PermissionList.cshtml", null);
            return null;
        }


        public IActionResult SetPermissionActive(long id)
        {
            Thread.Sleep(1000);
            return Json(new
            {
                Code = 0,
                Message = "保存成功",
                navTabId = "permissionlist"
            });
        }

        [HttpPost]
        public IActionResult ArticleTypeAdd(ArticleTypeAddSaveModel model)
        {
            return Json(new
            {
                Code = 0,
                Message = "保存成功",
                navTabId = "articletypelist"
            });
        }

        #endregion
    }
}