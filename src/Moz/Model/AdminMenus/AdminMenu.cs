﻿using SqlSugar;

namespace Moz.Bus.Models.AdminMenus
{
    [SugarTable("tab_admin_menu")]
    public class AdminMenu : BaseModel
    {
        public string Name { get; set; }
        
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }
        
        public string Link { get; set; }
        
        [SugarColumn(ColumnName = "order_index")]
        public int OrderIndex { get; set; }
        
        public string Icon { get; set; }
        
        [SugarColumn(ColumnName = "is_system")]
        public bool IsSystem { get; set; } 
        
        public string Path { get; set; }
    }
}