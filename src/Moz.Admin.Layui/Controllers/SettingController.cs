using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.Settings;
using Moz.Auth.Attributes;
using Moz.Bus.Services.Settings;
using Moz.Common.Types;
using Moz.Settings;

namespace Moz.Admin.Layui.Controllers
{
    /// <summary>
    /// 设置中心
    /// </summary>
    [AdminAuth(Permissions = "admin.setting")]
    public class SettingController : AdminBaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        /// <summary>
        /// 展示页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Index(string id)
        {
            var typeInfo = TypeFinder
                .FindClassesOfType<ISettings>()
                .FirstOrDefault(t => t.UniqueId == id);
            if (typeInfo == null)
                return Content("404");

            var setting = _settingService.LoadSetting(typeInfo.Type);

            var model = new IndexSettingRespModel
            {
                UniqueId = id,
                Title = typeInfo.DisplayName,
                TypeName = typeInfo.TypeName,
                SettingPropertyItems = typeInfo.Type.GetProperties().Select(t =>
                    {
                        var val = t.GetValue(setting)?.ToString();
                        if ((t.PropertyType == typeof(DateTime) || t.PropertyType == typeof(DateTime?)) && !string.IsNullOrEmpty(val))
                        {
                            if (DateTime.TryParse(val, out var dt))
                            {
                                val = dt.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }

                        var field = new SettingPropertyItem(t)
                        {
                            Value = val
                        };
                        
                        return field;
                    })
                    .ToList()
            };
            return View("~/Administration/Views/Setting/Index.cshtml", model);
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Save(SaveModel model)
        {
            var typeInfo = TypeFinder
                .FindClassesOfType<ISettings>()
                .FirstOrDefault(t => t.UniqueId == model.Id);

            if (typeInfo == null)
                return Json(new
                {
                    Code = 500,
                    Message = "找不到信息"
                });

            _settingService.SaveSetting(typeInfo.Type, model.Setting);

            return Json(new
            {
                Code = 0,
                Message = "保存成功"
            });
        }
    }
}