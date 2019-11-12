using Moz.Bus.Models;
using Moz.CMS.Model;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.Biz.Models.Categories
{
    [SugarTable("tab_category")]
    public class Category : BaseModel
    {
        public string Name { get; set; }
        
        public string Alias { get; set; }
        
        public string Desciption { get; set; }

        [SugarColumn(ColumnName = "order_index")]
        public int OrderIndex { get; set; } 
        
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }
    }
}