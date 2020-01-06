using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Models.Localization;
using Moz.CMS.Services.Settings;
using Moz.DataBase;
using Moz.Events;

namespace Moz.Bus.Services.Localization
{
    public class LanguageService : ILanguageService
    {
        #region Ctor

        public LanguageService(IDistributedCache distributedCache,
            ISettingService settingService,
            LocalizationSettings localizationSettings,
            IEventPublisher eventPublisher)
        {
            _distributedCache = distributedCache;
            _settingService = settingService;
            _localizationSettings = localizationSettings;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Fields

        private readonly IDistributedCache _distributedCache;
        private readonly ISettingService _settingService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="language"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            using (var client = DbFactory.GetClient())
            {
                var entity = client.Insertable(language).ExecuteReturnEntity();
                _eventPublisher.EntityDeleted(entity);
            }

            //_cachingManager.Remove<AddPolicy>();
        }

        /// <summary>
        /// </summary>
        /// <param name="language"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void UpdateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            using (var client = DbFactory.GetClient())
            {
                client.Updateable(language).ExecuteCommand();
            }

            //_cachingManager.Remove<UpdatePolicy>();
            _eventPublisher.EntityDeleted(language);
        }


        /// <summary>
        /// </summary>
        /// <param name="language"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void DeleteLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            using (var client = DbFactory.GetClient())
            {
                client.Deleteable(language).ExecuteCommand();
            }

            //_cachingManager.Remove<DeletePolicy>();
            _eventPublisher.EntityDeleted(language);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual IList<Language> GetAllLanguages()
        {
            return _distributedCache.GetOrSet("LANGUAGE_ALL_CACHE", () =>
            {
                using (var client = DbFactory.GetClient())
                {
                    return client.Queryable<Language>().ToList();
                }
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual Language GetLanguageById(long languageId)
        {
            using (var client = DbFactory.GetClient())
            {
                return client.Queryable<Language>().InSingle(languageId);
            }
        }

        #endregion
    }
}