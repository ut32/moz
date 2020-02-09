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
using Moz.Core.Options;
using Microsoft.OpenApi.Models;

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
                options.EncryptKey = "jEeESr7VySYru5c2";
                options.Admin.Path = "myadmin";
                options.Db.Add(new DbOptions
                {
                    MasterConnectionString = Configuration["ConnectionString"]
                });
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMoz(env);
        }
    }
}