using System;
using System.Linq.Expressions;
using Moz.Settings;

namespace Moz.Bus.Services.Settings
{
    public class SettingProperty<T>
        where T : ISettings, new()
    {
        private readonly ISettingService _service;

        public SettingProperty(ISettingService service)
        {
            _service = service;
        }

        public TPropType GetValue<TPropType>(Expression<Func<T, TPropType>> keySelector)
        {
            return _service.GetSettingValue(keySelector);
        }

        public void SetValue<TPropType>(Expression<Func<T, TPropType>> keySelector, TPropType value)
        {
            _service.SetSetting(keySelector, value);
        }
    }
}