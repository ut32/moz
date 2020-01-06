using System.Collections.Generic;
using Moz.Bus.Models.Localization;

namespace Moz.Bus.Services.Localization
{
    public interface ILanguageService
    {
        void DeleteLanguage(Language language);
        IList<Language> GetAllLanguages();
        Language GetLanguageById(long languageId);
        void InsertLanguage(Language language);
        void UpdateLanguage(Language language);
    }
}