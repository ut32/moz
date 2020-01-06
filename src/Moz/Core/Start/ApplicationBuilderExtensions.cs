using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moz.Aop.Middlewares;
using Moz.Bus.Dtos;
using Moz.Core;
using Moz.Core.Options;
using Moz.Exceptions;
using Moz.TaskSchedule;
using Moz.Utils.Types;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMoz(this IApplicationBuilder application,IWebHostEnvironment env)
        {
            var options = (application.ApplicationServices.GetService(typeof(IOptions<MozOptions>)) as IOptions<MozOptions>)?.Value;
            if(options == null)
                throw new ArgumentNullException(nameof (options));
            
            if (env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseExceptionHandler("/error/500");
            }

            application.UseMiddleware<ErrorHandlingMiddleware>();

            application.UseStatusCodePages(async context =>
            {
                var registerType = options.StatusCodePageHandlerType;
                if (registerType != null &&
                    application.ApplicationServices.GetService(registerType) is IStatusCodePageHandler handler)
                {
                    await handler.Process(context.HttpContext);
                }
                else
                {
                    if (application.ApplicationServices.GetService(typeof(MozStatusCodePageHandler)) is IStatusCodePageHandler mozHandler)
                    {
                        await mozHandler.Process(context.HttpContext);
                    }
                }
            });
            
            application.UseMiddleware<JwtInHeaderMiddleware>();
            
            //定时任务
            if(options.IsEnableScheduling)
                application.UseTaskSchedule();
            
            application.UseMozStaticFiles();
            
            application.UseAuthentication();
            
            application.UseSession();

            application.UseRouting();

            application.UseAuthorization();

            //获取所有的 IMozStartup,执行各个模块的启动类
            var startupConfigurations = TypeFinder.FindClassesOfType<IMozStartup>();
            var instances = startupConfigurations
                .Select(startup => (IMozStartup)Activator.CreateInstance(startup.Type))
                .OrderBy(startup => startup.Order);
            foreach (var instance in instances) 
                instance.Configure(application, env);
            
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            application.Run(context =>
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            });
        }

        private static void UseTaskSchedule(this IApplicationBuilder application)
        {
            TaskScheduleManager.Instance.Init().GetAwaiter().GetResult();
        }

        private static void UseMozStaticFiles(this IApplicationBuilder application)
        {
            application.UseStaticFiles();
  
            var assemblies = DependencyContext.Default.CompileLibraries
                .Where(t => t.Dependencies.Any(x => "moz".Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                .Select(t => Assembly.Load(t.Name)).ToList();
            foreach (var assembly in assemblies)
            {
                var fileProvider = new EmbeddedFileProvider(assembly);
                application.UseStaticFiles(new StaticFileOptions {FileProvider = fileProvider});
            }
        }
        
    }
}