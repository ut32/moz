using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moz.Common;
using Moz.Configuration;
using Moz.Core;
using Moz.Core.Options;
using Moz.Utils;

namespace Moz.Administration.Common
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