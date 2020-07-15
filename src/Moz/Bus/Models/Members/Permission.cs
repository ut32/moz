
using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     permission
    /// </summary>
    [SugarTable("tab_permission")]
    public class Permission : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "is_active")]
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "is_system")]
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "order_index")]
        public int OrderIndex { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        #endregion
    }
}