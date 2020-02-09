using System.Collections.Generic;

namespace Moz.Bus.Models.AdminMenus
{
    public class AdminMenuTree
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<AdminMenuTree> Children { get; set; }
    }
}