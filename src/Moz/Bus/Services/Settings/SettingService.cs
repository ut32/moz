using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Models.Configuration;
using Moz.DataBase;
using Moz.Events;
using Moz.Settings;
using Moz.Utils;

namespace Moz.Bus.Services.Settings
{
    public class SettingService : ISettingService
    {
        
        private const string SETTING_CACHE_KEY_ALL = "SETTING_CACHE_KEY_ALL";
        
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="eventPublisher"></param>
        public SettingService(IDistributedCache distributedCache,
            IEventPublisher eventPublisher
        )
        {
            _distributedCache = distributedCache;
            _eventPublisher = eventPublisher;
        }

        

        #region Utilities

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Setting> GetAllSettingsCached()
        {
            return _distributedCache.GetOrSet(SETTING_CACHE_KEY_ALL, () =>
            {
                using (var client = DbFactory.GetClient())
                {
                    return client.Queryable<Setting>().ToList();
                }
            });
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        protected virtual List<Setting> GetAllSettings()
        {
            using (var client = DbFactory.GetClient())
            { 
                return client.Queryable<Setting>().ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="keySelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPropType"></typeparam>
        /// <returns></returns>
        protected virtual bool SettingExists<T, TPropType>(Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var key = keySelector.GetPropName()?.Trim()?.ToLowerInvariant() ?? "";
            return GetAllSettings().FirstOrDefault(t => t.Name.Trim().ToLowerInvariant() == key) == null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void InsertSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            using (var client = DbFactory.GetClient())
            {
                setting.Id = client.Insertable(setting).ExecuteReturnBigIdentity();
            }
            _distributedCache.Remove(SETTING_CACHE_KEY_ALL);
            _eventPublisher.EntityCreated(setting);
        }

        /// <summary>
        /// </summary>
        /// <param name="setting"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void UpdateSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            using (var client = DbFactory.GetClient())
            {
                client.Updateable(setting).ExecuteCommand();
            }
            _distributedCache.Remove(SETTING_CACHE_KEY_ALL);
            _eventPublisher.EntityUpdated(setting);
        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        public virtual void DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            using (var client = DbFactory.GetClient())
            {
                client.Deleteable(setting).ExecuteCommand();
            }

            _distributedCache.Remove(SETTING_CACHE_KEY_ALL);
            _eventPublisher.EntityDeleted(setting);
        }

        /// <summary>
        /// Deletes settings
        /// </summary>
        /// <param name="settings">Settings</param>
        public virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (settings.Count == 0)
                return;

            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<Setting>(settings).ExecuteCommand();
            }

            _distributedCache.Remove(SETTING_CACHE_KEY_ALL);
            foreach (var setting in settings) _eventPublisher.EntityDeleted(setting);
        }


        #region GetSetting

        public virtual Setting GetSetting(long settingId)
        {
            var settings = GetAllSettingsCached();
            return settings.FirstOrDefault(o => o.Id == settingId);
        }

        public virtual Setting GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsCached();

            return settings.FirstOrDefault(o => o.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region GetSettingValue

        public virtual TPropType GetSettingValue<T, TPropType>(Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            return GetSettingValue(keySelector, default(TPropType));
        }

        public virtual TPropType GetSettingValue<T, TPropType>(Expression<Func<T, TPropType>> keySelector,
            TPropType defaultValue)
            where T : ISettings, new()
        {
            var key = keySelector?.GetPropName() ?? "";
            return GetSettingValue(key, defaultValue);
        }

        public virtual T GetSettingValue<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            var setting = settings.FirstOrDefault(o => o.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (setting == null) return defaultValue;
            return ConvertHelper.To<T>(setting.Value);
        }

        #endregion

        #region SetSetting    

        public virtual void SetSetting<T, TPropType>(Expression<Func<T, TPropType>> keySelector, TPropType value)
            where T : ISettings, new()
        {
            var key = keySelector.GetPropName() ?? "";
            SetSetting(key, value);
        }

        public virtual void SetSetting<T>(string key, T value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));


            var valueStr = TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettings();
            var setting = allSettings.FirstOrDefault(o => o.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (setting != null)
            {
                setting.Value = valueStr;
                UpdateSetting(setting);
            }
            else
            {
                setting = new Setting
                {
                    Name = key,
                    Value = valueStr
                };
                InsertSetting(setting);
            }
        }

        #endregion

        #region LoadSetting

        /// <inheritdoc />
        /// <summary>
        ///     Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual T LoadSetting<T>() where T : ISettings, new()
        {
            return (T) LoadSetting(typeof(T));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Load settings
        /// </summary>
        /// <param name="type">Type</param>
        public virtual ISettings LoadSetting(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;

                var setting = GetSetting(key);
                //Console.WriteLine($"key:{key},value:{setting?.Value??""}");
                if (setting == null || setting.Value.IsNullOrEmpty())
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting.Value))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting.Value);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        #endregion

        #region SaveSetting

        /// <summary>
        /// </summary>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        public virtual void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = (typeof(T).Name + "." + prop.Name).ToLowerInvariant();
                dynamic value = prop.GetValue(settings, null);
                SetSetting(key, value ?? "");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dictionary"></param>
        public virtual void SaveSetting(Type type, Dictionary<string, string> dictionary)
        {
            var settings = GetAllSettings();
            var insertSettings = new List<Setting>();
            var updateSettings = new List<Setting>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var name = propertyInfo.Name;
                var key = (type.Name + "." + name).Trim();
                
                if(!dictionary.ContainsKey(key))
                    continue;
                
                var value = dictionary[key];

                if (!TypeDescriptor.GetConverter(propertyInfo.PropertyType).CanConvertFrom(typeof(string)))
                    continue;
                if (!TypeDescriptor.GetConverter(propertyInfo.PropertyType).IsValid(value))
                    continue;

                var setting = settings.FirstOrDefault(t => t.Name.Equals(key));
                if (setting == null)
                {
                    insertSettings.Add(new Setting
                    {
                        Name = key,
                        Value = value
                    });
                }
                else
                {
                    setting.Value = value;
                    updateSettings.Add(setting);
                }
            }

            if (insertSettings.Count > 0)
                using (var client = DbFactory.GetClient())
                {
                    client.Insertable(insertSettings).ExecuteCommand();
                    _distributedCache.Remove(SETTING_CACHE_KEY_ALL);
                }

            if (updateSettings.Count > 0)
                using (var client = DbFactory.GetClient())
                {
                    client.Updateable(updateSettings).ExecuteCommand();
                    _distributedCache.Remove(SETTING_CACHE_KEY_ALL);
                }
        }

        #endregion

        #region DeleteSetting

        /// <summary>
        ///     Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                var key = (typeof(T).Name + "." + prop.Name).ToLowerInvariant();
                var setting = allSettings.FirstOrDefault(o => o.Name.Trim().ToLowerInvariant() == key);
                if (setting != null) settingsToDelete.Add(setting);
            }

            DeleteSettings(settingsToDelete);
        }

        /// <summary>
        /// </summary>
        /// <param name="keySelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPropType"></typeparam>
        public virtual void DeleteSetting<T, TPropType>(Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var key = keySelector.GetPropName()?.Trim()?.ToLowerInvariant() ?? "";
            var allSettings = GetAllSettings();
            var setting = allSettings.FirstOrDefault(o => o.Name.Trim().ToLowerInvariant() == key);
            if (setting != null) DeleteSetting(setting);
        }

        #endregion
        

        #endregion
    }
}