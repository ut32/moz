using Microsoft.AspNetCore.Mvc;
using Moz.Administration.Common;

namespace Moz.Admin.Layui.Controllers
{
    public class UserController : AdminBaseController
    {
        // GET
        public IActionResult Login()
        {
            return Json(new {code = 20000, data = new {token = "admin"}});
        }

        public IActionResult Info()
        {
            return Json(new {code = 20000, data = new {role = "admin", name = "admin", avatar = "admin"}});
        }
    }
}