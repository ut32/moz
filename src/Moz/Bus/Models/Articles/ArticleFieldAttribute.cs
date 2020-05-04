using System;
using Moz.Common.Form;

namespace Moz.Bus.Models.Articles
{
    public class ArticleFieldAttribute:Attribute
    {
        public ArticleFieldAttribute(string name, FormFieldType fieldType, bool multiLanguage = false)
        {
            Name = name;
            FieldType = fieldType;
            MultiLanguage = multiLanguage;
        }

        public FormFieldType FieldType { get; }
        public string Name { get; }
        public bool MultiLanguage { get; }
    }
}