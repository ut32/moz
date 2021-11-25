using System;
using System.Linq.Expressions;
using Moz.Bus.Models;
using Moz.Bus.Models.Localization;

namespace Moz.Bus.Services.Localization
{
    public interface ILocalizedEntityService
    {
        void DeleteLocalizedProperty(LocalizedProperty localizedProperty);

        LocalizedProperty GetLocalizedPropertyById(long localizedPropertyId);

        string GetLocalizedValue(long languageId, long entityId, string localeKeyGroup, string localeKey);


        void InsertLocalizedProperty(LocalizedProperty localizedProperty);


        void UpdateLocalizedProperty(LocalizedProperty localizedProperty);


        void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            long languageId) where T : BaseModel, ILocalizedEntity;


        void SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            long languageId) where T : BaseModel, ILocalizedEntity;
    }
}