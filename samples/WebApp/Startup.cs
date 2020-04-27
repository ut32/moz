using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moz.Core.Config;
using SqlSugar;
using WebApp.Models;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMoz(options =>
            { 
                /*
                 ==全局配置==
                 AppSecret: 涉及到应用安全，必须配置，长度为16-32位
                 IsEnableScheduling: 是否开启定时任务，可选配置，开启后后台才可操作，默认未开启。
                 IsEnablePerformanceMonitor: 是否开启性能监视，可选配置，开启后后台才有数据，默认未开启。
                 */
                options.AppSecret = "jEeESr7VySYru5c2jEeESr7VySYru5c2";
                options.IsEnableScheduling = false;
                options.IsEnablePerformanceMonitor = false;

                /*
                 ==后台配置==
                 Path: 后台路径，可选配置
                 LoginView: 后台自定义登录页，可选配置
                 WelcomeView: 后台自定义首页，可选配置
                 */
                options.Admin.Path = "myadmin";
                options.Admin.LoginView = "";
                options.Admin.WelcomeView = "";
                
                /*
                 ==数据库配置==
                 使用以下方式配置，默认数据库类型为MySql,名称为Default。
                 如果有多数据库，依次在后边添加，但主要须指定不同名称。
                 */
                options.Db.Add(new DbConfig
                {
                    MasterConnectionString = Configuration["ConnectionString"]
                });
                
                /*
                 ==错误页配置==
                 错误页包括程序抛错，鉴权失败，404等。以下配置均为可选配置
                 DefaultRedirect: 错误默认跳转地址
                 HttpErrors: 状态码配置表
                 
                //options.ErrorPage.DefaultRedirect = "/error/{0}??{2}";
                //Execute 模式不支持 ？
                options.ErrorPage.HttpErrors = new List<HttpError>
                {
                    new HttpError{ StatusCode = 404, Path = "/error/{0}", Mode = ResponseMode.Execute}
                };*/
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMoz(env);
        }
    }
}