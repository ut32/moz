using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models.Settings;
using Moz.CMS.Services.Settings;
using Moz.Configuration;
using Moz.Domain.Services.Settings;
using Moz.Utils.Types;

namespace Moz.Administration.Controllers
{
    public class SettingController : AdminBaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IActionResult Index(string id)
        {
            var typeInfo = TypeFinder
                .FindClassesOfType<ISettings>()
                .FirstOrDefault(t => t.Guid == id);
            if (typeInfo == null) 
                return Content("404");

            var setting = _settingService.LoadSetting(typeInfo.Type);

            var model = new IndexSettingRespModel
            {
                Guid = id,
                Title = typeInfo.DisplayName,
                TypeName = typeInfo.Name,
                PropertiesItems = typeInfo.Type.GetProperties().Select(t => new SettingPropertiesItem
                {
                    DisplayName = GetPropertyDisplayName(t),
                    Name = t.Name,
                    Value = t.GetValue(setting)?.ToString(),
                    PropertType = GetPropertType(t)
                }).ToList()
            };

            return View("~/Administration/Views/Setting/Index.cshtml", model);
        }

        private PropertType GetPropertType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(string)) return PropertType.STRING;

            if (propertyInfo.PropertyType == typeof(bool)) return PropertType.BOOL;

            if (propertyInfo.PropertyType == typeof(DateTime)) return PropertType.DATETIME;

            if (propertyInfo.PropertyType == typeof(int)) return PropertType.INT;

            if (propertyInfo.PropertyType == typeof(float)) return PropertType.FLOAT;

            if (propertyInfo.PropertyType == typeof(double)) return PropertType.DOUBLE;

            if (propertyInfo.PropertyType == typeof(decimal)) return PropertType.DECIMAL;

            return PropertType.STRING;
        }

        private string GetPropertyDisplayName(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return "--";
            var displayAttr = propertyInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttr == null ? propertyInfo.Name : displayAttr.Name;
        }

        public IActionResult Save(SaveModel model)
        {
            var typeInfo = TypeFinder
                .FindClassesOfType<ISettings>()
                .FirstOrDefault(t => t.Guid == model.Id);

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