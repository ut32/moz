using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models.Members;
using Moz.Auth;
using Moz.Bus.Dtos.Auth;
using Moz.Core.Config;
using Moz.Exceptions;
using Moz.Settings;

namespace Moz.Admin.Layui.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IAuthService _authService;
        private readonly AdminSettings _adminSettings;

        public HomeController(IAuthService passportService,
            AdminSettings adminSettings)
        {
            _authService = passportService;
            _adminSettings = adminSettings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var path = string.IsNullOrEmpty(_adminSettings.AdminLoginView) 
                ? "~/Administration/Views/Home/Index.cshtml"
                : _adminSettings.AdminLoginView;           
            return View(path);
        }
        
        
        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            var result = _authService.LoginWithUsernamePassword(new LoginWithUsernamePasswordDto
            {
                Username = model.Username,
                Password = model.Password
            }); 
            if(result.Code>0)
                throw new AlertException(result.Message);
            _authService.SetAuthCookie(new SetAuthCookieDto()
            {
                Token = result.Data.AccessToken
            });
            return RespJson(new{});
        }
        

        [HttpGet]
        public IActionResult Logout()
        {
            _authService.RemoveAuthCookie();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult Error()
        {
            return View("~/Administration/Views/Home/Error.cshtml");
        }
    }
}