using System;
using Moz.Common.Form;

namespace Moz.Bus.Models.Articles
{
    [Serializable]
    public class ArticleConfiguration
    {
        public string FiledName { get; set; }
        public string DisplayName { get; set; }
        public bool IsEnable { get; set; }
        public FormFieldType DisplayType { get; set; }
        
        public string Options { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        
        public bool IsMultiLanguage { get; set; }
        public bool IsShowedInList { get; set; }
        public bool IsEnableSearch { get; set; }
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
    }
}