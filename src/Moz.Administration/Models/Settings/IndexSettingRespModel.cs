using System.Collections.Generic;

namespace Moz.Administration.Models.Settings
{
    public class IndexSettingRespModel
    {
        public string Title { get; set; }
        public string Guid { get; set; }
        public string TypeName { get; set; }
        public List<SettingPropertiesItem> PropertiesItems { get; set; }
    }

    public class SettingPropertiesItem
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public PropertType PropertType { get; set; }
    }

    public enum PropertType
    {
        STRING = 1,
        BOOL = 2,
        DATETIME = 3,
        FLOAT = 4,
        INT = 5,
        DOUBLE = 6,
        DECIMAL = 7
    }
}