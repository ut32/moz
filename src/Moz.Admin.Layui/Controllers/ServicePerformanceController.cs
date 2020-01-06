using System;﻿
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Exceptions;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Bus.Dtos.ServicePerformances;
using Moz.Bus.Models.Common;
using Moz.Bus.Services.ServicePerformances;
using Moz.DataBase;

namespace Moz.Administration.Controllers
{
    [AdminAuth(Permissions = "admin.performanceMonitor")]
    public class ServicePerformanceController : AdminAuthBaseController
    {
        private readonly IServicePerformanceService _servicePerformanceService;
        public ServicePerformanceController(IServicePerformanceService servicePerformanceService)
        {
            this._servicePerformanceService = servicePerformanceService;
        }

        [AdminAuth(Permissions = "admin.performanceMonitor.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.ServicePerformances.IndexModel();
            return View("~/Administration/Views/ServicePerformance/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.performanceMonitor.index")]
        public IActionResult PagedList(PagedQueryServicePerformanceRequest request)
        {
            var list = _servicePerformanceService.PagedQueryServicePerformances(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.performanceMonitor.deleteall")]
        public IActionResult DeleteAll()
        {
            using (var db = DbFactory.GetClient())
            {
                db.Deleteable<ServicePerformanceMonitor>().ExecuteCommand();
            }
            return RespJson(new{});
        }
    }
}
