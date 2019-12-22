using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Administration.Common;
using Moz.Administration.Models.Main;
using Moz.Configuration;
using Moz.Core;
using Moz.Utils.Types;

namespace Moz.Admin.Layui.Controllers
{
    public class MainController : AdminAuthBaseController
    {
        private readonly IWorkContext _workContext;

        public MainController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public IActionResult Index()
        {
            var settingTypeInfos = TypeFinder.FindClassesOfType<ISettings>();

            var model = new IndexRspModel
            {
                AdminUserName = _workContext.CurrentMember.Username,
                SettingMenus = settingTypeInfos.Select(t => new SettingMenu
                {
                    Id = t.Guid,
                    Name = t.DisplayName
                }).ToList()
            };
            return View("~/Administration/Views/Main/Index.cshtml", model);
        }
    }
}