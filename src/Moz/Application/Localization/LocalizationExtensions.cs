using System;
using System.Linq.Expressions;
using System.Reflection;
using Moz.Bus.Models;
using Moz.Bus.Models.Localization;
using Moz.Bus.Models.Members;
using Moz.Bus.Services.Settings;
using Moz.Core;
using Moz.Settings;
using Moz.Utils;

namespace Moz.Bus.Services.Localization
{
    public static class LocalizationExtensions
    {
        /// <summary>
        ///     Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>Localized property</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector)
            where T : BaseModel, ILocalizedEntity
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return GetLocalized(entity, keySelector, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        ///     Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">
        ///     A value indicating whether to ensure that we have at least two published
        ///     languages; otherwise, load only default value
        /// </param>
        /// <returns>Localized property</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector, long languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : BaseModel, ILocalizedEntity
        {
            return GetLocalized<T, string>(entity, keySelector, languageId, returnDefaultValue,
                ensureTwoPublishedLanguages);
        }

        /// <summary>
        ///     Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">
        ///     A value indicating whether to ensure that we have at least two published
        ///     languages; otherwise, load only default value
        /// </param>
        /// <returns>Localized property</returns>
        public static TPropType GetLocalized<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector, long languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : BaseModel, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var member = keySelector.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

            var result = default(TPropType);
            var resultStr = string.Empty;

            var localeKeyGroup = entity.GetType().Name;
            var localeKey = propInfo.Name;

            if (languageId > 0)
            {
                //ensure that we have at least two published languages
                var loadLocalizedValue = true;
                if (ensureTwoPublishedLanguages)
                {
                    var lService = EngineContext.Current.Resolve<ILanguageService>();
                    var totalPublishedLanguages = lService.GetAllLanguages().Count;
                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                //localized value
                if (loadLocalizedValue)
                {
                    var leService = EngineContext.Current.Resolve<ILocalizedEntityService>();
                    resultStr = leService.GetLocalizedValue(languageId, entity.Id, localeKeyGroup, localeKey);
                    if (!string.IsNullOrEmpty(resultStr))
                        result = ConvertHelper.To<TPropType>(resultStr);
                }
            }

            //set default value if required
            if (string.IsNullOrEmpty(resultStr) && returnDefaultValue)
            {
                var localizer = keySelector.Compile();
                result = localizer(entity);
            }

            return result;
        }

        /// <summary>
        ///     Get localized property of setting
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">
        ///     A value indicating whether to ensure that we have at least two published
        ///     languages; otherwise, load only default value
        /// </param>
        /// <returns>Localized property</returns>
        public static string GetLocalizedSetting<T>(this T settings,
            Expression<Func<T, string>> keySelector, long languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : ISettings, new()
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();

            var key = SettingExtensions.GetSettingKey(settings, keySelector);

            //we do not support localized settings per store (overridden store settings)
            var setting = settingService.GetSetting(key);
            if (setting == null)
                return null;

            return null;
            //return setting.GetLocalized(x => x.Value, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        ///     Save localized property of setting
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="value">Localized value</param>
        /// <returns>Localized property</returns>
        public static void SaveLocalizedSetting<T>(this T settings,
            Expression<Func<T, string>> keySelector, long languageId,
            string value)
            where T : ISettings, new()
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();
            var localizedEntityService = EngineContext.Current.Resolve<ILocalizedEntityService>();

            var key = SettingExtensions.GetSettingKey(settings, keySelector);

            var setting = settingService.GetSetting(key);
            if (setting == null)
                return;

            localizedEntityService.SaveLocalizedValue(setting, x => x.Value, value, languageId);
        }

        /// <summary>
        ///     Get localized value of enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="workContext">Work context</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService,
            IWorkContext workContext)
            where T : struct
        {
            if (workContext == null)
                throw new ArgumentNullException(nameof(workContext));

            return GetLocalizedEnum(enumValue, localizationService, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        ///     Get localized value of enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService,
            long languageId)
            where T : struct
        {
            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));

            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            //localized value
            var resourceName = $"Enums.{typeof(T)}.{enumValue}";
            var result = localizationService.GetResource(resourceName, languageId, false, "", true);

            //set default value if required
            if (string.IsNullOrEmpty(result))
                result = ConvertHelper.ConvertEnum(enumValue.ToString());

            return result;
        }

        /// <summary>
        ///     Get localized value of permission
        ///     We don't have UI to manage permission localizable name. That's why we're using this extension method
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="workContext">Work context</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedPermissionName(this Permission permissionRecord,
            ILocalizationService localizationService, IWorkContext workContext)
        {
            if (workContext == null)
                throw new ArgumentNullException(nameof(workContext));

            return GetLocalizedPermissionName(permissionRecord, localizationService, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        ///     Get localized value of enum
        ///     We don't have UI to manage permission localizable name. That's why we're using this extension method
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized value</returns>
        public static string GetLocalizedPermissionName(this Permission permissionRecord,
            ILocalizationService localizationService, long languageId)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));

            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));

            //localized value
            var resourceName = $"Permission.{permissionRecord.Code}";
            var result = localizationService.GetResource(resourceName, languageId, false, "", true);

            //set default value if required
            if (string.IsNullOrEmpty(result))
                result = permissionRecord.Name;

            return result;
        }

        /// <summary>
        ///     Save localized name of a permission
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageService">Language service</param>
        public static void SaveLocalizedPermissionName(this Permission permissionRecord,
            ILocalizationService localizationService, ILanguageService languageService)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));
            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));
            if (languageService == null)
                throw new ArgumentNullException(nameof(languageService));

            var resourceName = $"Permission.{permissionRecord.Code}";
            var resourceValue = permissionRecord.Name;

            foreach (var lang in languageService.GetAllLanguages())
            {
                var lsr = localizationService.GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr == null)
                {
                    lsr = new LocaleStringResource
                    {
                        LanguageId = lang.Id,
                        Name = resourceName,
                        Value = resourceValue
                    };
                    localizationService.InsertLocaleStringResource(lsr);
                }
                else
                {
                    lsr.Value = resourceValue;
                    localizationService.UpdateLocaleStringResource(lsr);
                }
            }
        }

        /// <summary>
        ///     Delete a localized name of a permission
        /// </summary>
        /// <param name="permissionRecord">Permission record</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageService">Language service</param>
        public static void DeleteLocalizedPermissionName(this Permission permissionRecord,
            ILocalizationService localizationService, ILanguageService languageService)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));
            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));
            if (languageService == null)
                throw new ArgumentNullException(nameof(languageService));

            var resourceName = $"Permission.{permissionRecord.Code}";
            foreach (var lang in languageService.GetAllLanguages())
            {
                var lsr = localizationService.GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr != null)
                    localizationService.DeleteLocaleStringResource(lsr);
            }
        }
        
    }
}