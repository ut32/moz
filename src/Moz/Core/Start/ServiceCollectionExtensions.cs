using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Auth.Handlers;
using Moz.CMS.Services.Settings;
using Moz.Core;
using Moz.Core.Attributes;
using Moz.Core.Config;
using Moz.Core.WorkContext;
using Moz.DataBase;
using Moz.Events;
using Moz.Events.Publishers;
using Moz.Exceptions;
using Moz.Settings;
using Moz.TaskSchedule;
using Moz.Utils;
using Moz.Utils.FileManager;
using Moz.Utils.Types;
using Moz.Validation;
using Quartz;
using Quartz.Spi;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    
    public static class ServiceCollectionExtensions
    {

        public static void AddMoz(this IServiceCollection services, Action<AppConfig> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            services.Configure(configure);

            //获取配置
            var buildServiceProvider = services.BuildServiceProvider();
            var configuration = buildServiceProvider.GetService<IConfiguration>();
            var webHostEnvironment = buildServiceProvider.GetService<IWebHostEnvironment>();

            //验证appConfig
            var appConfig = buildServiceProvider.GetService<IOptions<AppConfig>>();
            if (appConfig?.Value == null)
                throw new ArgumentNullException(nameof(AppConfig));

            //必须配置EncryptKey
            if (appConfig.Value.AppSecret.IsNullOrEmpty())
                throw new Exception(nameof(appConfig.Value.AppSecret));

            //必须为16-32位
            if (appConfig.Value.AppSecret.Length < 16 || appConfig.Value.AppSecret.Length>32)
                throw new Exception("加密KEY位数不正确，必须为16-32位");

            //检查是否已安装数据库
            DbFactory.CheckInstalled(appConfig.Value);
            
            var serviceProvider = ConfigureServices(services, configuration, webHostEnvironment, appConfig.Value);
            EngineContext.Create(serviceProvider);
        }

        private static IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, AppConfig mozOptions)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin_authorize",
                    policy => { policy.Requirements.Add(new DefaultAuthorizationRequirement("admin")); });
                options.AddPolicy("member_authorize",
                    policy => { policy.Requirements.Add(new DefaultAuthorizationRequirement("member")); });
            });

            services.AddAuthentication()
                .AddJwtBearer(MozAuthAttribute.MozAuthorizeSchemes, cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    var parameters = EngineContext.Current.Resolve<IJwtService>().GetTokenValidationParameters();
                    cfg.TokenValidationParameters = parameters;

                    cfg.Events = new JwtBearerEvents
                    {
                        //OnAuthenticationFailed = o => throw new AlertException("auth failure")
                    };
                });

            //Session会话
            services.AddSession(options =>
            {
                options.Cookie.Name = "__moz__session";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            //添加MVC
            services.AddMvc(options => { })
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; })
                .AddFluentValidation(options =>
                {
                    options.ImplicitlyValidateChildProperties = true;
                    options.ValidatorFactoryType = typeof(MozValidatorFactory);
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });



            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            #region 依赖注入

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IWorkContext, WebWorkContext>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddTransient<HttpContextHelper>();
            services.AddSingleton<IEventPublisher, DefaultEventPublisher>();
            services.AddSingleton<ITaskScheduleManager, TaskScheduleManager>();
            services.AddSingleton<IAuthorizationHandler, DefaultAuthorizationHandler>();
            services.AddSingleton<IJobFactory, JobFactory>();

            //注入服务类 查找所有Service结尾的类进行注册
            var allServiceInterfaces = TypeFinder.GetAllTypes()
                .Where(t => (t?.IsInterface ?? false) && !t.IsDefined<IgnoreRegisterAttribute>(false) &&
                            t.Name.EndsWith("Service"));
            foreach (var serviceInterface in allServiceInterfaces)
            {
                var service = TypeFinder.FindClassesOfType(serviceInterface.Type)?.FirstOrDefault();
                if (service != null) services.AddTransient(serviceInterface.Type, service.Type);
            }
            
            //注入所有Job类
            var jobTypes = TypeFinder.FindClassesOfType<IJob>().ToList();
            foreach (var jobType in jobTypes)
                services.AddSingleton(jobType.Type);
            


            //注册settings
            var settingTypes = TypeFinder.FindClassesOfType(typeof(ISettings)).ToList();
            foreach (var settingType in settingTypes)
                services.TryAddTransient(settingType.Type, serviceProvider =>
                {
                    if (DbFactory.CheckInstalled(mozOptions))
                    {
                        var settingService = serviceProvider.GetService<ISettingService>();
                        return settingService.LoadSetting(settingType.Type);
                    }

                    var instance = Activator.CreateInstance(settingType.Type);
                    return instance;
                });


            //注入 ExceptionHandler
            var exceptionHandlers = TypeFinder.FindClassesOfType(typeof(IExceptionHandler))
                .Where(it => it.Type != typeof(ErrorHandlingMiddleware))
                .ToList();
            foreach (var exceptionHandler in exceptionHandlers)
                services.AddSingleton(exceptionHandler.Type);

            //注入 StatusCodePageHandler
            var statusCodePageHandlers = TypeFinder.FindClassesOfType(typeof(IStatusCodePageHandler)).ToList();
            foreach (var statusCodePageHandler in statusCodePageHandlers)
                services.AddSingleton(statusCodePageHandler.Type);

            //事件发布
            var consumerTypes = TypeFinder.FindClassesOfType(typeof(ISubscriber<>)).ToList();
            foreach (var consumerType in consumerTypes)
            {
                var interfaceTypes = consumerType.Type.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType &&
                                  ((Type) criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(ISubscriber<>));
                var interfaceType = interfaceTypes.FirstOrDefault();
                if (interfaceType != null) services.AddTransient(interfaceType, consumerType.Type);
            }

            #endregion

            //获取所有的 IAppStartup
            var startupConfigurations = TypeFinder.FindClassesOfType<IAppStartup>();

            //添加嵌入cshtml资源
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                foreach (var cfg in startupConfigurations)
                    options.FileProviders.Add(new EmbeddedFileProvider(cfg.Type.GetTypeInfo().Assembly));
            });

            //执行各个模块的启动类
            var instances = startupConfigurations
                .Select(startup => (IAppStartup) Activator.CreateInstance(startup.Type))
                .OrderBy(startup => startup.Order);
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration, webHostEnvironment, mozOptions);

            //services.
            return services.BuildServiceProvider();
        }
    }
}