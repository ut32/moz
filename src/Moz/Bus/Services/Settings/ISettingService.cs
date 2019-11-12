using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moz.CMS.Model.Configuration;
using Moz.Configuration;

namespace Moz.CMS.Services.Settings
{
    public interface ISettingService
    {
        void InsertSetting(Setting setting);
        void UpdateSetting(Setting setting);
        void DeleteSetting(Setting setting);
        void DeleteSettings(IList<Setting> settings);

        Setting GetSetting(long settingId);
        Setting GetSetting(string key);

        TPropType GetSettingValue<T, TPropType>(Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new();

        TPropType GetSettingValue<T, TPropType>(Expression<Func<T, TPropType>> keySelector, TPropType defaultValue)
            where T : ISettings, new();

        T GetSettingValue<T>(string key, T defaultValue = default(T));


        void SetSetting<T, TPropType>(Expression<Func<T, TPropType>> keySelector, TPropType value)
            where T : ISettings, new();

        void SetSetting<T>(string key, T value);


        T LoadSetting<T>() where T : ISettings, new();
        ISettings LoadSetting(Type type);

        void SaveSetting<T>(T settings) where T : ISettings, new();
        void SaveSetting(Type type, Dictionary<string, string> dictionary);

        void DeleteSetting<T>() where T : ISettings, new();
        void DeleteSetting<T, TPropType>(Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
    }
}