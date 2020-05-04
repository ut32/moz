using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models.Main;
using Moz.Auth.Attributes;
using Moz.Common.Types;
using Moz.Core;
using Moz.Settings;

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
                SettingMenus = settingTypeInfos
                    .OrderBy(it => it.Order)
                    .Select(t => new SettingMenu
                    {
                        Id = t.UniqueId,
                        Name = t.DisplayName
                    }).ToList()
            };
            return View("~/Administration/Views/Main/Index.cshtml", model);
        }

        public IActionResult Welcome()
        {
            return View("~/Administration/Views/Main/Index.cshtml");
        }
    }
}