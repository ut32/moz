using System.Collections.Generic;
using Moz.Bus.Models.AdminMenus;

namespace Moz.Bus.Dtos.AdminMenus
{
    public class QueryChildrenByParentIdDto
    {
         public long? ParentId { get; set; }
    }

    public class QueryChildrenByParentIdApo
    {
        public List<AdminMenuTree> AllSubs { get; set; }
    }
}