using System.Collections.Generic;
using Moz.Bus.Models.Localization;
using Moz.CMS.Model.Localization;

namespace Moz.Bus.Services.Localization
{
    public interface ILocalizationService
    {
        void DeleteLocaleStringResource(LocaleStringResource localeStringResource);
        LocaleStringResource GetLocaleStringResourceById(long localeStringResourceId);
        LocaleStringResource GetLocaleStringResourceByName(string resourceName);

        LocaleStringResource GetLocaleStringResourceByName(string resourceName, long languageId,
            bool logIfNotFound = true);

        List<LocaleStringResource> GetAllResources(long languageId);
        void InsertLocaleStringResource(LocaleStringResource localeStringResource);
        void UpdateLocaleStringResource(LocaleStringResource localeStringResource);
        Dictionary<string, KeyValuePair<long, string>> GetAllResourceValues(long languageId);

        string GetResource(string resourceKey);

        string GetResource(string resourceKey, long languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false);

        string ExportResourcesToXml(Language language);
        void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true);
    }
}