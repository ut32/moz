using System.Collections.Generic;

namespace Moz.Admin.Layui.Models.Settings
{
    public class SaveModel
    {
        public string Id { get; set; }

        //public dynamic Setting { get; set; } 为空
        //public object Setting { get; set; } 为空
        //public JObject Setting { get; set; } 报错
        public Dictionary<string, string> Setting { get; set; }
    }
}