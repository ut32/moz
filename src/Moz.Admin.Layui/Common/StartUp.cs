using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moz.Core;
using Moz.Core.Options;

namespace Moz.Admin.Layui.Common
{
    public class StartUp: IMozStartup
    {
        public int Order { get; } = 90;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment, MozOptions mozOptions)
        {
            
        }

        public void Configure(IApplicationBuilder application, IConfiguration configuration, IWebHostEnvironment webHostEnvironment,
            MozOptions mozOptions)
        {
            
        }
    }
}