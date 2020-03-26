using System;
using System.Linq.Expressions;
using System.Reflection;
using Moz.CMS.Services.Settings;
using Moz.Settings;

namespace Moz.Domain.Services.Settings
{
    public static class SettingExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keySelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPropType"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetSettingKey<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    keySelector));

            var key = typeof(T).Name + "." + propInfo.Name;
            return key;
        }

        public static SettingProperty<T> Use<T>(this ISettingService service)
            where T : ISettings, new()
        {
            var settingProperty = new SettingProperty<T>(service);
            return settingProperty;
        }
    }
}