using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moz.Bus.Models.Localization;
using Moz.Core;
using Moz.DataBase;
using Moz.Events;
using Moz.Settings;

namespace Moz.Bus.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="workContext"></param>
        /// <param name="commonSettings"></param>
        /// <param name="localizationSettings"></param>
        /// <param name="eventPublisher"></param>
        /// <param name="logger"></param>
        public LocalizationService(IDistributedCache distributedCache,
            IWorkContext workContext,
            CommonSettings commonSettings,
            LocalizationSettings localizationSettings,
            IEventPublisher eventPublisher,
            ILogger<LocalizationService> logger)
        {
            _distributedCache = distributedCache;
            _workContext = workContext;
            _commonSettings = commonSettings;
            _localizationSettings = localizationSettings;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        #endregion

        #region Utilities

        private static Dictionary<string, KeyValuePair<long, string>> ResourceValuesToDictionary(
            IEnumerable<LocaleStringResource> locales)
        {
            var dictionary = new Dictionary<string, KeyValuePair<long, string>>();
            foreach (var locale in locales)
            {
                var resourceName = locale.Name.ToLowerInvariant();
                if (!dictionary.ContainsKey(resourceName))
                    dictionary.Add(resourceName, new KeyValuePair<long, string>(locale.Id, locale.Value));
            }

            return dictionary;
        }

        #endregion

        #region Constants

        private const string ALL_RESOURCE_VALUES_BY_LANGUAGE_ID = "ALL_RESOURCE_VALUES_BY_LANGUAGE_{0}";

        #endregion

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IDistributedCache _distributedCache;
        private readonly CommonSettings _commonSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<LocalizationService> _logger;

        #endregion

        #region Methods

        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException(nameof(localeStringResource));

            using (var client = DbFactory.GetClient())
            {
                client.Insertable(localeStringResource).ExecuteCommand();
            }

            //_cacheManager.Remove<AllResources>();
            _eventPublisher.EntityCreated(localeStringResource);
        }
        
        public virtual void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException(nameof(localeStringResource));

            using (var client = DbFactory.GetClient())
            {
                client.Updateable(localeStringResource).ExecuteCommand();
            }

            //_cacheManager.Remove<AllResources>();
            _eventPublisher.EntityUpdated(localeStringResource);
        }

        protected virtual void InsertLocaleStringResources(List<LocaleStringResource> resources)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            using (var client = DbFactory.GetClient())
            {
                client.Insertable(resources).ExecuteCommand();
            }

            //_cacheManager.Remove<AllResources>();
            foreach (var resource in resources) _eventPublisher.EntityCreated(resource);
        }

        

        protected virtual void UpdateLocaleStringResources(List<LocaleStringResource> resources)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            using (var client = DbFactory.GetClient())
            {
                client.Updateable(resources).ExecuteCommand();
            }

            //_cacheManager.Remove<AllResources>();
            foreach (var resource in resources) _eventPublisher.EntityCreated(resource);
        }

        public virtual void DeleteLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException(nameof(localeStringResource));

            using (var client = DbFactory.GetClient())
            {
                client.Deleteable(localeStringResource).ExecuteCommand();
            }

            //_cacheManager.Remove<AllResources>();
            _eventPublisher.EntityDeleted(localeStringResource);
        }


        public virtual LocaleStringResource GetLocaleStringResourceById(long localeStringResourceId)
        {
            using (var client = DbFactory.GetClient())
            {
                return client.Queryable<LocaleStringResource>().InSingle(localeStringResourceId);
            }
        }


        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName)
        {
            if (_workContext.WorkingLanguage != null)
                return GetLocaleStringResourceByName(resourceName, _workContext.WorkingLanguage.Id);

            return null;
        }


        public virtual LocaleStringResource GetLocaleStringResourceByName(string name, long languageId,
            bool logIfNotFound = true)
        {
            using (var client = DbFactory.GetClient())
            {
                var localeStringResource = client.Queryable<LocaleStringResource>().First(lsr =>
                    lsr.LanguageId == languageId && lsr.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (localeStringResource == null && logIfNotFound)
                    _logger.LogWarning($"Resource string ({name}) not found. Language ID = {languageId}");

                return localeStringResource;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public virtual List<LocaleStringResource> GetAllResources(long languageId)
        {
            using (var client = DbFactory.GetClient())
            {
                return client.Queryable<LocaleStringResource>().Where(lan => lan.LanguageId == languageId).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public virtual Dictionary<string, KeyValuePair<long, string>> GetAllResourceValues(long languageId)
        {
            var lsr = _distributedCache.GetOrSet(string.Format(ALL_RESOURCE_VALUES_BY_LANGUAGE_ID, languageId), () =>
            {
                using (var client = DbFactory.GetClient())
                {
                    return client.Queryable<LocaleStringResource>().Where(it => it.LanguageId == languageId)
                        .ToList();
                }
            });
            
            return ResourceValuesToDictionary(lsr);
        }

        /// <summary>
        ///     Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey)
        {
            if (_workContext.WorkingLanguage != null)
                return GetResource(resourceKey, _workContext.WorkingLanguage.Id);

            return "";
        }

        /// <summary>
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="languageId"></param>
        /// <param name="logIfNotFound"></param>
        /// <param name="defaultValue"></param>
        /// <param name="returnEmptyIfNotFound"></param>
        /// <returns></returns>
        public virtual string GetResource(string resourceKey, long languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            var result = string.Empty;
            if (resourceKey == null) resourceKey = string.Empty;
            resourceKey = resourceKey.Trim().ToLowerInvariant();

            var resources = GetAllResourceValues(languageId);
            if (resources.ContainsKey(resourceKey)) result = resources[resourceKey].Value;

            if (result.IsNullOrEmpty())
            {
                if (logIfNotFound)
                    _logger.LogWarning($"Resource string ({resourceKey}) is not found. Language ID = {languageId}");

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    result = defaultValue;
                }
                else
                {
                    if (!returnEmptyIfNotFound)
                        result = resourceKey;
                }
            }

            return result;
        }

        /// <summary>
        ///     Export language resources to XML
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportResourcesToXml(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Language");
            xmlWriter.WriteAttributeString("Name", language.Name);
            xmlWriter.WriteAttributeString("SupportedVersion", "1");

            var resources = GetAllResources(language.Id);
            foreach (var resource in resources)
            {
                xmlWriter.WriteStartElement("LocaleResource");
                xmlWriter.WriteAttributeString("Name", resource.Name);
                xmlWriter.WriteElementString("Value", null, resource.Value);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        ///     Import language resources from XML file
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="xml">XML</param>
        /// <param name="updateExistingResources">A value indicating whether to update existing resources</param>
        public virtual void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (string.IsNullOrEmpty(xml))
                return;
            //stored procedures aren't supported
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var nodes = xmlDoc.SelectNodes(@"//Language/LocaleResource");

            var existingResources = GetAllResources(language.Id);
            var newResources = new List<LocaleStringResource>();

            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["Name"].InnerText.Trim();
                var value = "";
                var valueNode = node.SelectSingleNode("Value");
                if (valueNode != null)
                    value = valueNode.InnerText;

                if (string.IsNullOrEmpty(name))
                    continue;

                //do not use "Insert"/"Update" methods because they clear cache
                //let's bulk insert
                var resource = existingResources.FirstOrDefault(x =>
                    x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                if (resource != null)
                {
                    if (updateExistingResources) resource.Value = value;
                }
                else
                {
                    newResources.Add(
                        new LocaleStringResource
                        {
                            LanguageId = language.Id,
                            Name = name,
                            Value = value
                        });
                }
            }

            InsertLocaleStringResources(newResources);
            UpdateLocaleStringResources(existingResources);


            //clear cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        #endregion
    }
}