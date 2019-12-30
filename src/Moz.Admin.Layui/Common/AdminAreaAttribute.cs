using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Core;
using Moz.Core.Options;

namespace Moz.Admin.Layui.Common
{
    public class AdminAreaAttribute : AreaAttribute
    {
        public AdminAreaAttribute()
            : base(GetAdminPath())
        {
        
        }

        private static string GetAdminPath()
        {
            return EngineContext.Current.Resolve<IOptions<MozOptions>>()?.Value?.Admin.Path ?? "myadmin";
        }
    }
}