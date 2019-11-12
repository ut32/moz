using System.Collections.Generic;

namespace Moz.Administration.Models.Roles
{
    public class ConfigMenuModel
    {
        public long RoleId { get; set; }
        public List<ConfigMenuItem> Menus { get; set; }
    }
    
    public class ConfigMenuItem
    {
        public long id { get; set; }
        public string name { get; set; }
        public long pId { get; set; }
        public bool @checked { get; set; }
        public bool open { get; set; }
    }
}