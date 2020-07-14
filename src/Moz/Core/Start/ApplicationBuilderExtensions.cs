using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moz.Aop.Middlewares;
using Moz.Bus.Dtos;
using Moz.Common.Types;
using Moz.Core;
using Moz.Core.Config;
using Moz.DataBase;
using Moz.Exceptions;
using Moz.TaskSchedule;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseMoz(this IApplicationBuilder application,IWebHostEnvironment env)
        {
            var configuration = application.ApplicationServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var options = (application.ApplicationServices.GetService(typeof(IOptions<AppConfig>)) as IOptions<AppConfig>)?.Value;
            if(options == null)
                throw new ArgumentNullException(nameof (options));
            
            if (env.IsDevelopment())
                application.UseDeveloperExceptionPage();

            application.UseMiddleware<ErrorHandlingMiddleware>();

            
            application.UseStatusCodePages(async context =>
            {
                var registerType = options.StatusCodePageHandlerType;
                if (registerType != null && application.ApplicationServices.GetService(registerType) is IStatusCodePageHandler handler)
                {
                    await handler.Process(context);
                }
                else
                {
                    if (application.ApplicationServices.GetService(typeof(MozStatusCodePageHandler)) is IStatusCodePageHandler mozHandler)
                    {
                        await mozHandler.Process(context);
                    }
                }
            });
            
           
            
            application.UseMiddleware<JwtInHeaderMiddleware>();
            
            //定时任务
            if (DbFactory.CheckInstalled(options))
            {
                var taskScheduleManager = application.ApplicationServices.GetService(typeof(ITaskScheduleManager)) as ITaskScheduleManager;
                taskScheduleManager?.Init();
            }

            application.UseMozStaticFiles();
            
            application.UseAuthentication();
            
            application.UseSession();

            application.UseRouting();

            application.UseAuthorization();

            //获取所有的 IAppStartup,执行各个模块的启动类
            var startupConfigurations = TypeFinder.FindClassesOfType<IAppStartup>();
            var instances = startupConfigurations
                .Select(startup => (IAppStartup)Activator.CreateInstance(startup.Type))
                .OrderBy(startup => startup.Order);
            foreach (var instance in instances) 
                instance.Configure(application,configuration, env, options);
            
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