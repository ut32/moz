using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Auth.Attributes;
using Moz.Auth.Handlers;
using Moz.CMS.Services.Settings;
using Moz.Configuration;
using Moz.Core;
using Moz.Core.Attributes;
using Moz.Core.Options;
using Moz.Core.WorkContext;
using Moz.Events;
using Moz.Events.Publishers;
using Moz.Exceptions;
using Moz.Utils;
using Moz.Utils.FileManager;
using Moz.Utils.Types;
using Moz.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    
    public static class ServiceCollectionExtensions
    {

        public static IServiceProvider AddMoz(this IServiceCollection services, Action<MozOptions> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof (services));
            
            if (configure == null)
                throw new ArgumentNullException(nameof (configure));
            
            services.Configure(configure);
            
            //get system configuration
            var buildServiceProvider = services.BuildServiceProvider();
            var configuration = buildServiceProvider.GetService<IConfiguration>();

            //valid configuration
            var options = buildServiceProvider.GetService<IOptions<MozOptions>>();
            if(options?.Value == null)
                throw new ArgumentNullException(nameof (MozOptions));
            
            //required key 
            if(options.Value.EncryptKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof (options.Value.EncryptKey));
            
            if(options.Value.EncryptKey.Length!=16)
                throw new ArgumentException("Encrypt Key's length must equal 16");
            
            //check database connection string
            if (!options.Value.Db.Any(t=> "default".Equals(t.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentNullException(nameof(options.Value.Db));

            var serviceProvider = ConfigureServices(services,configuration,options.Value); 
            EngineContext.Create(serviceProvider);
            
            return serviceProvider;
        }

        private static IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration,
            MozOptions mozOptions)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin_authorize",
                    policy => { policy.Requirements.Add(new AdminAuthorizationHandler()); });
                options.AddPolicy("member_authorize",
                    policy => { policy.Requirements.Add(new MemberAuthorizationHandler()); });
            });

            services.AddAuthentication()
                .AddJwtBearer(MozAuthAttribute.MozAuthorizeSchemes, cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "https://ut32.com",
                        ValidAudience = "moz_application",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mozOptions.EncryptKey))
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = o => throw new AlertException("auth failure")
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
            services.AddMvc(options =>
                {
                   
                })
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
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

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddTransient<IWorkContext, WebWorkContext>();
            services.TryAddSingleton<IFileManager, FileManager>();
            services.TryAddTransient<HttpContextHelper>();
            services.TryAddSingleton<IEventPublisher, DefaultEventPublisher>();

            //注入服务类 查找所有Service结尾的类进行注册
            var allServiceInterfaces = TypeFinder.GetAllTypes()
                .Where(t => (t?.IsInterface ?? false) && !t.IsDefined<IgnoreRegisterAttribute>(false) &&
                            t.Name.EndsWith("Service"));
            foreach (var serviceInterface in allServiceInterfaces)
            {
                var service = TypeFinder.FindClassesOfType(serviceInterface.Type)?.FirstOrDefault();
                if (service != null) services.TryAddTransient(serviceInterface.Type, service.Type);
            }


            //注册settings
            var settingService = services.BuildServiceProvider().GetService<ISettingService>();
            var settingTypes = TypeFinder.FindClassesOfType(typeof(ISettings)).ToList();
            foreach (var settingType in settingTypes)
                services.TryAddTransient(settingType.Type, o => settingService.LoadSetting(settingType.Type));

            //注入 ExceptionHandler
            var exceptionHandlers = TypeFinder.FindClassesOfType(typeof(IExceptionHandler))
                .Where(it=>it.Type!=typeof(ErrorHandlingMiddleware))
                .ToList();
            foreach (var exceptionHandler in exceptionHandlers)
                services.TryAddSingleton(exceptionHandler.Type);
            
            //注入 StatusCodePageHandler
            var statusCodePageHandlers = TypeFinder.FindClassesOfType(typeof(IStatusCodePageHandler)).ToList();
            foreach (var statusCodePageHandler in statusCodePageHandlers)
                services.TryAddSingleton(statusCodePageHandler.Type);
            
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
                if (interfaceType != null) services.TryAddTransient(interfaceType, consumerType.Type);
            }

            #endregion



            //获取所有的 IMozStartup
            var startupConfigurations = TypeFinder.FindClassesOfType<IMozStartup>();

            //添加嵌入cshtml资源
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                foreach (var cfg in startupConfigurations)
                    options.FileProviders.Add(new EmbeddedFileProvider(cfg.Type.GetTypeInfo().Assembly));
            });

            //执行各个模块的启动类
            var instances = startupConfigurations
                .Select(startup => (IMozStartup) Activator.CreateInstance(startup.Type))
                .OrderBy(startup => startup.Order);
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration, mozOptions);

            return services.BuildServiceProvider();
        }
    }
}