using System.Collections.Generic;
using Moz.Settings;

namespace Moz.Admin.Layui.Models.Settings
{
    public class IndexSettingRespModel
    {
        public string Title { get; set; }
        public string UniqueId { get; set; } 
        public string TypeName { get; set; }
        public List<SettingPropertyItem> SettingPropertyItems { get; set; }
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