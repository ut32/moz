using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Core;
using Moz.Core.Options;

namespace Moz.Admin.Api.Common
{
    public class AdminApiRouteAttribute:RouteAttribute
    {
        private static readonly string AdminPath;
        public AdminApiRouteAttribute() :  
            base($"{AdminPath}/api/v{{version:apiVersion}}/[controller]") 
        {
        }

        static AdminApiRouteAttribute()
        {
            AdminPath = EngineContext.Current.Resolve<IOptions<MozOptions>>()?.Value?.Admin.Path ?? "myadmin";
        }
    }
}