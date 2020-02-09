using System.Collections.Generic;

namespace Moz.Bus.Models.Members
{

    public class PermissionTree
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public List<PermissionTree> Children { get; set; }
    }
}