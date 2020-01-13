using SqlSugar;

namespace Moz.Bus.Models.Categories
{
    [SugarTable("tab_category")]
    public class Category : BaseModel
    {
        public string Name { get; set; }
        
        public string Alias { get; set; }
        
        public string Description { get; set; } 

        [SugarColumn(ColumnName = "order_index")]
        public int OrderIndex { get; set; } 
        
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }
        
        public string Path { get; set; }
    }
}