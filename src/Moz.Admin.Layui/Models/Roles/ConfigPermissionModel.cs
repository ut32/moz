using System.Collections.Generic;

namespace Moz.Administration.Models.Roles
{
    public class ConfigPermissionModel
    {
        public long RoleId { get; set; }
        public List<ConfigPermissionItem> Permissions { get; set; }
    }
    
    public class ConfigPermissionItem
    {
        public long id { get; set; }
        public string name { get; set; }
        public long pId { get; set; }
        public bool @checked { get; set; }
        public bool open { get; set; }
    }
}