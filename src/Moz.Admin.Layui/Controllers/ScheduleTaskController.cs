using System;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Biz.Dtos.ScheduleTasks;
using Moz.Bus.Dtos.ScheduleTasks;
using Moz.CMS.Services.ScheduleTasks;
using Moz.Exceptions;
using Moz.Utils.Types;
using Quartz;

namespace Moz.Administration.Controllers
{
    [AdminAuth(Permissions = "admin.scheduleTask")]
    public class ScheduleTaskController : AdminAuthBaseController
    {
        private readonly IScheduleTaskService _scheduleTaskService;
        public ScheduleTaskController(IScheduleTaskService scheduleTaskService)
        {
            _scheduleTaskService = scheduleTaskService;
        }

        [AdminAuth(Permissions = "admin.scheduleTask.index")]
        public IActionResult Index()
        {
            var model = new Models.ScheduleTasks.IndexModel();
            return View("~/Administration/Views/ScheduleTask/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.scheduleTask.index")]
        public IActionResult PagedList(PagedQueryScheduleTaskRequest request)
        {
            var list = _scheduleTaskService.PagedQueryScheduleTasks(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.scheduleTask.create")]
        public IActionResult Create()
        {
            var types = TypeFinder
                .FindClassesOfType<IJob>()
                .Select(t=>new { t.DisplayName, t.FullName})
                .ToList();

            var model = new Moz.Administration.Models.ScheduleTasks.CreateModel
            {
                Types = types.Select(t=>
                {
                    dynamic d = new ExpandoObject();
                    d.FullName = t.FullName;
                    d.DisplayName = t.DisplayName;
                    return d;
                }).ToList()
            };
            return View("~/Administration/Views/ScheduleTask/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.create")]
        public IActionResult Create(Moz.Biz.Dtos.ScheduleTasks.CreateScheduleTaskRequest request)
        {
            var resp = _scheduleTaskService.CreateScheduleTask(request);
            return RespJson(resp);
        }
        
        [AdminAuth(Permissions = "admin.scheduleTask.update")]
        public IActionResult Update(Moz.Biz.Dtos.ScheduleTasks.GetScheduleTaskDetailRequest request)
        {
            var scheduleTask = _scheduleTaskService.GetScheduleTaskDetail(request);
            if (scheduleTask == null)
            {
                throw new MozException("信息不存在，可能被删除");
            }
            var model = new  Moz.Administration.Models.ScheduleTasks.UpdateModel()
            {
                ScheduleTask = scheduleTask
            };
            return View("~/Administration/Views/ScheduleTask/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.update")]
        public IActionResult Update(Moz.Biz.Dtos.ScheduleTasks.UpdateScheduleTaskRequest request)
        {
            var resp = _scheduleTaskService.UpdateScheduleTask(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.delete")]
        public IActionResult Delete(Moz.Biz.Dtos.ScheduleTasks.DeleteScheduleTaskRequest request)
        {
            var resp = _scheduleTaskService.DeleteScheduleTask(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.setisenable")]
        public IActionResult SetIsEnable(SetIsEnableScheduleTaskRequest request)
        {
            var resp = _scheduleTaskService.SetIsEnableScheduleTask(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.execute")]
        public IActionResult Execute(ExecuteScheduleTaskRequest request)
        {
            var resp = _scheduleTaskService.ExecuteScheduleTask(request);
            return RespJson(resp);
        }
    }
}
