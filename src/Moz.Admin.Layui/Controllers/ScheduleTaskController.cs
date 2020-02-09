using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.ScheduleTasks;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.ScheduleTasks;
using Moz.Bus.Services.ScheduleTasks;
using Moz.Core.Options;
using Moz.Exceptions;
using Moz.Utils.Types;
using Quartz;

namespace Moz.Admin.Layui.Controllers
{
    [AdminAuth(Permissions = "admin.scheduleTask")]
    public class ScheduleTaskController : AdminAuthBaseController
    {
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly MozOptions _mozOptions;
        public ScheduleTaskController(IScheduleTaskService scheduleTaskService,IOptions<MozOptions> mozOptions)
        {
            _scheduleTaskService = scheduleTaskService;
            _mozOptions = mozOptions.Value;
        }

        [AdminAuth(Permissions = "admin.scheduleTask.index")]
        public IActionResult Index()
        {
            var model = new IndexModel();
            if(_mozOptions.IsEnableScheduling)
                return View("~/Administration/Views/ScheduleTask/Index.cshtml",model);
            return View("~/Administration/Views/ScheduleTask/Disable.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.scheduleTask.index")]
        public IActionResult PagedList(PagedQueryScheduleTaskDto dto) 
        {
            var list = _scheduleTaskService.PagedQueryScheduleTasks(dto);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.Data.TotalCount,
                Data = list.Data.List
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

            var model = new CreateModel
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
        public IActionResult Create(CreateScheduleTaskDto dto)
        {
            var result = _scheduleTaskService.CreateScheduleTask(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.scheduleTask.update")]
        public IActionResult Update(GetScheduleTaskDetailDto dto) 
        {
            var result = _scheduleTaskService.GetScheduleTaskDetail(dto);
            if (result.Code > 0)
            {
                return Json(result);
            }
            var model = new UpdateModel()
            {
                ScheduleTask = result.Data
            };
            return View("~/Administration/Views/ScheduleTask/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.update")]
        public IActionResult Update(UpdateScheduleTaskDto dto)
        {
            var result = _scheduleTaskService.UpdateScheduleTask(dto); 
            return Json(result); 
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.delete")]
        public IActionResult Delete(DeleteScheduleTaskDto dto)
        {
            var result = _scheduleTaskService.DeleteScheduleTask(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.setIsEnable")]
        public IActionResult SetIsEnable(SetIsEnableScheduleTaskDto dto)
        {
            var result = _scheduleTaskService.SetIsEnableScheduleTask(dto);
            return Json(result); 
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.scheduleTask.execute")]
        public IActionResult Execute(ExecuteScheduleTaskDto dto)
        {
            var result = _scheduleTaskService.ExecuteScheduleTask(dto);
            return Json(result); 
        }
    }
}
