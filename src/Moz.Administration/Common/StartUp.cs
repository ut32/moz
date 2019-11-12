
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moz.Core;
using Moz.Core.Options;

namespace Moz.Administration.Common
{
    public class StartUp: IMozStartup
    {
        public int Order { get; } = 99;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, MozOptions mozOptions)
        {
            
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
        {
            
        }
    }
}