using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Models;
using Moz.Bus.Models.Localization;
using Moz.DataBase;
using Moz.Utils;

namespace Moz.Bus.Services.Localization
{
    public class LocalizedEntityService : ILocalizedEntityService
    {
        #region Ctor

        public LocalizedEntityService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="localeKeyGroup"></param>
        /// <returns></returns>
        protected virtual IList<LocalizedProperty> GetLocalizedProperties(long entityId, string localeKeyGroup)
        {
            if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
                return new List<LocalizedProperty>();

            using (var client = DbFactory.CreateClient())
            {
                return client.Queryable<LocalizedProperty>()
                    .Where(lp => lp.EntityId == entityId && lp.LocaleKeyGroup == localeKeyGroup).OrderBy(o => o.Id)
                    .ToList();
            }
        }

        #endregion

        #region Fields

        private readonly IDistributedCache _distributedCache;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="localizedProperty"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException(nameof(localizedProperty));

            using (var client = DbFactory.CreateClient())
            {
                client.Insertable(localizedProperty).ExecuteCommand();
            }

            //_cacheManager.Remove<AddPolicy>();
        }

        /// <summary>
        /// </summary>
        /// <param name="localizedProperty"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void UpdateLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException(nameof(localizedProperty));

            using (var client = DbFactory.CreateClient())
            {
                client.Updateable(localizedProperty).ExecuteCommand();
            }

            var key =
                $"{localizedProperty.LanguageId}_{localizedProperty.EntityId}_{localizedProperty.LocaleKeyGroup}_{localizedProperty.LocaleKey}";
            //_cacheManager.Remove<UpdatePolicy>(key);
        }

        /// <summary>
        /// </summary>
        /// <param name="localizedProperty"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void DeleteLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException(nameof(localizedProperty));

            using (var client = DbFactory.CreateClient())
            {
                client.Deleteable(localizedProperty).ExecuteCommand();
            }

            var key =
                $"{localizedProperty.LanguageId}_{localizedProperty.EntityId}_{localizedProperty.LocaleKeyGroup}_{localizedProperty.LocaleKey}";
            //_cacheManager.Remove<DeletePolicy>(key);
        }

        /// <summary>
        ///     Gets a localized property
        /// </summary>
        /// <param name="localizedPropertyId">Localized property identifier</param>
        /// <returns>Localized property</returns>
        public virtual LocalizedProperty GetLocalizedPropertyById(long localizedPropertyId)
        {
            using (var client = DbFactory.CreateClient())
            {
                return client.Queryable<LocalizedProperty>().InSingle(localizedPropertyId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="entityId"></param>
        /// <param name="localeKeyGroup"></param>
        /// <param name="localeKey"></param>
        /// <returns></returns>
        public virtual string GetLocalizedValue(long languageId, long entityId, string localeKeyGroup, string localeKey)
        {
            /*
            var key = $"{languageId}_{entityId}_{localeKeyGroup}_{localeKey}";
            var value = _cacheManager
                .Get("CACHE_LOCAL_ENTITY")
                .ExpireIn(100)
                .WithRemovePolicy<UpdatePolicy>(key)
                .WithRemovePolicy<DeletePolicy>(key)
                .WhenNotFound(() =>
                {
                    using (var client = DbFactory.GetClient())
                    {
                        var entity = client.Queryable<LocalizedProperty>()
                            .First(lp => lp.LanguageId == languageId
                                         && lp.EntityId == entityId
                                         && lp.LocaleKeyGroup.Equals(localeKeyGroup, StringComparison.OrdinalIgnoreCase)
                                         && lp.LocaleKey.Equals(localeKey, StringComparison.OrdinalIgnoreCase));
                        return entity == null ? "" : entity.LocaleValue;
                    }
                });
            return value;
            */
            return string.Empty;
        }


        /// <summary>
        ///     Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public virtual void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            long languageId) where T : BaseModel, ILocalizedEntity
        {
            SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }

        /// <summary>
        ///     Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        public virtual void SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            long languageId) where T : BaseModel, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "Language ID should not be 0");

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

            //load localized value (check whether it's a cacheable entity. In such cases we load its original entity type)
            var localeKeyGroup = entity.GetType().Name;
            var localeKey = propInfo.Name;

            var props = GetLocalizedProperties(entity.Id, localeKeyGroup);
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                                                  lp.LocaleKey.Equals(localeKey,
                                                      StringComparison
                                                          .InvariantCultureIgnoreCase)); //should be culture invariant

            var localeValueStr = ConvertHelper.To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    DeleteLocalizedProperty(prop);
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    UpdateLocalizedProperty(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //insert
                    prop = new LocalizedProperty
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        LocaleKey = localeKey,
                        LocaleKeyGroup = localeKeyGroup,
                        LocaleValue = localeValueStr
                    };
                    InsertLocalizedProperty(prop);
                }
            }
        }

        #endregion
    }
}