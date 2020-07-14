using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Admin.Layui.Common;
using Moz.Core.Config;
using Moz.Settings;

namespace Moz.Admin.Layui.Controllers
{
    public class AdminWelcomeViewComponent:ViewComponent
    {
        private readonly AdminSettings _adminSettings;
        
        
        public AdminWelcomeViewComponent(AdminSettings globalSettings)
        {
            _adminSettings = globalSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(0);
            var path = string.IsNullOrEmpty(_adminSettings.AdminWelcomeView) 
                ? "~/Administration/Views/Main/Welcome.cshtml"
                : _adminSettings.AdminWelcomeView;
            return View(path);
        } 
    }
}