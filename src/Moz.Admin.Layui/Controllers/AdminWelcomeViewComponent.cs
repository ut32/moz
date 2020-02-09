using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Core.Options;

namespace Moz.Admin.Layui.Controllers
{
    public class AdminWelcomeViewComponent:ViewComponent
    {
        private readonly IOptions<MozOptions> _mozOptions;
        
        
        public AdminWelcomeViewComponent(IOptions<MozOptions> mozOptions)
        {
            _mozOptions = mozOptions;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(0);
            var path = string.IsNullOrEmpty(_mozOptions.Value.Admin.WelcomeView) 
                ? "~/Administration/Views/Main/Welcome.cshtml"
                : _mozOptions.Value.Admin.WelcomeView;
            return View(path);
        } 
    }
}