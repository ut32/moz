using System.Collections.Generic;

namespace Moz.Administration.Models.Main
{
    public class IndexRspModel
    {
        public string AdminUserName { get; set; }
        public List<SettingMenu> SettingMenus { get; set; }
    }

    public class SettingMenu
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}