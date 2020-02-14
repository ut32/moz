using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Moz.Bus.Models.Common;
using Moz.Core;
using Moz.DataBase;
using Moz.Exceptions;

namespace Moz.Aop.Interceptor
{
    public class ServicePerformanceInterceptorAttribute : AbstractInterceptorAttribute
    {

        private IHttpContextAccessor HttpContextAccessor { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                await next(context);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                var name = $"{context.ImplementationMethod.DeclaringType.Name}.{context.ImplementationMethod.Name}";

                Task.Run(() =>
                {
                    var httpContextAccessor = EngineContext.Current.Resolve<IHttpContextAccessor>();
                    var requestId = httpContextAccessor?.HttpContext?.TraceIdentifier;
                    using (var client = DbFactory.GetClient())
                    {
                        var servicePerformance = new ServicePerformanceMonitor();
                        servicePerformance.Name = name;
                        servicePerformance.ElapsedMs = (int) elapsedMs;
                        servicePerformance.HttpRequestId = requestId;
                        servicePerformance.AddTime = DateTime.Now;
                        servicePerformance.Id = client.Insertable(servicePerformance).ExecuteReturnBigIdentity();
                    }
                });

            }
            catch (MozException zoException)
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}