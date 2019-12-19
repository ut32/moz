using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using AspectCore.Configuration;
using AspectCore.DynamicProxy.Parameters;
using AspectCore.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Aop.Filters;
using Moz.Aop.Interceptor;
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

            services.AddAuthentication().AddJwtBearer(MozAuthAttribute.MozAuthorizeSchemes, cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "https://136cc.com",
                    ValidAudience = "moz_application",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mozOptions.EncryptKey))
                };
                cfg.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = o => throw new MozException("auth failure", 401)
                };
            });


            //添加分布式缓存
            //var distributionModel = mozOptions?.DistributionOption?.Model;
            if (false)
            {
                /*
                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = configuration["redis:connectionString"];
                    options.InstanceName = configuration["redis:instanceName"];
                });
                */
            }
            else
            {
                //services.AddDistributedMemoryCache();
            }

            //Session会话
            services.AddSession(options =>
            {
                options.Cookie.Name = "__moz__session";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            //services.AddSignalR();
            /*
            services.AddSwaggerGen(c =>
            {
                ///var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                c.SwaggerDoc("v1", new Info {Title = "My API", Version = "1.0"});
                c.SwaggerDoc("v2", new Info {Title = "My API", Version = "2.0"});
                // add a custom operation filter which sets default values
                //c.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                //c.IncludeXmlComments( XmlCommentsFilePath );
            });
            
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    //options.SubstituteApiVersionInUrl = true;
                });
            services.AddApiVersioning(option =>
            {
                //reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                option.ReportApiVersions = false;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });
            */

            services.AddMvc(config =>
                {
                    if (mozOptions.IsEnablePerformanceMonitor)
                    {
                        config.Filters.Add(typeof(PerformanceMonitorFilter));
                    }

                    config.Filters.Add(typeof(SearchExceptionHandlerFilter));
                })
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .AddFluentValidation(config =>
                {
                    config.ImplicitlyValidateChildProperties = true;
                    config.ValidatorFactoryType = typeof(MozValidatorFactory);
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });
                //.AddMvcRazorRuntimeCompilation()
                //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

                services.AddApiVersioning(o => {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                });

            #region 依赖注入

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddTransient<IWorkContext, WebWorkContext>();
            services.TryAddSingleton<IFileManager,FileManager>();
            services.TryAddTransient<HttpContextHelper>();

            if (""?.Equals("redis", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                services.TryAddSingleton<IEventPublisher, RedisEventPublisher>();
            }
            else
            {
                services.TryAddSingleton<IEventPublisher, DefaultEventPublisher>();
            }

            //注入服务类 查找所有Service结尾的类进行注册
            var allServiceInterfaces = TypeFinder.GetAllTypes()
                .Where(t => (t?.IsInterface ?? false) && !t.IsDefined<IgnoreRegisterAttribute>(false)  && t.Name.EndsWith("Service"));
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


            //event consumers
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
            /*
            services.Configure<RazorViewEngineOptions>(options =>
            {
                foreach (var cfg in startupConfigurations)
                    options.FileProviders.Add(new EmbeddedFileProvider(cfg.Type.GetTypeInfo().Assembly));
            });
            */
            
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


            //AOP拦截
            /*
            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<ValidateInterceptorAttribute>(Predicates.ForService("*Service"));
            });
            */
            return services.BuildServiceProvider();
        }
    }
}