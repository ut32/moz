using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moz.Core.Config;

namespace Moz.Core
{
    public interface IAppStartup
    {
        int Order { get; }  
        
        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, AppConfig appConfig);   
         
        void Configure(IApplicationBuilder application, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, AppConfig appConfig);   
    } 
}