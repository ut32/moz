using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moz.Core.Options;

namespace Moz.Core
{
    public interface IMozStartup
    {
        int Order { get; }  
        
        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, MozOptions mozOptions);   
         
        void Configure(IApplicationBuilder application, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, MozOptions mozOptions);  
    } 
}