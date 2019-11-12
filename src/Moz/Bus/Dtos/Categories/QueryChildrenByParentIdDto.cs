using System.Collections.Generic;

namespace Moz.Biz.Dtos.Categories
{
    public class QueryChildrenByParentIdRequest
    {
         public long? ParentId { get; set; }
    }

    public class QueryChildrenByParentIdResponse
    {
        public List<SimpleCategory> AllSubs { get; set; }
    }

    public class SimpleCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<SimpleCategory> Children { get; set; }
    }
}