using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Moz.Bus.Models.Common;
using Moz.Core;
using Moz.DataBase;

namespace Moz.Aop.Filters
{
    public class PerformanceMonitorFilter:IActionFilter
    {
        private Stopwatch _stopwatch;
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var elapsedMs = _stopwatch.ElapsedMilliseconds;
            var name = context.ActionDescriptor.DisplayName;
            Task.Run(() =>
            {
                var httpContextAccessor = EngineContext.Current.Resolve<IHttpContextAccessor>();
                var requestId = httpContextAccessor?.HttpContext?.TraceIdentifier;
                using (var client = DbFactory.CreateClient())
                {
                    var servicePerformance = new ServicePerformanceMonitor
                    {
                        Name = name, 
                        ElapsedMs = (int) elapsedMs, 
                        HttpRequestId = requestId,
                        AddTime = DateTime.Now
                    };
                    servicePerformance.Id = client.Insertable(servicePerformance).ExecuteReturnBigIdentity();
                }
            });
        }
    }
}