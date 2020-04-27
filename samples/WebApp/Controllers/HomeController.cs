using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moz.Auth.Attributes;
using Moz.Exceptions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            return Content("登录页面");
        }

        [Route("notfound")]
        public IActionResult NotFound()
        {
            return Content("没有找到数据");
        }

        [MemberAuth]
        public IActionResult Privacy()
        {
            //throw new AlertException("用户名不能为空");
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            return Content("错误页面");
        }
    }
}