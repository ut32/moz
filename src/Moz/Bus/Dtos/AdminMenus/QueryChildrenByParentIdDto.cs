using System.Collections.Generic;

namespace Moz.Bus.Dtos.AdminMenus
{
    public class QueryChildrenByParentIdRequest
    {
         public long? ParentId { get; set; }
    }

    public class QueryChildrenByParentIdResponse
    {
        public List<SimpleAdminMenu> AllSubs { get; set; }
    }

    public class SimpleAdminMenu
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<SimpleAdminMenu> Children { get; set; }
    }
}