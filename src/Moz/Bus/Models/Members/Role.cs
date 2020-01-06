
using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     role
    /// </summary>
    [SugarTable("tab_role")]
    public class Role : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "is_active")]
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "is_admin")]
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "is_system")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// </summary>
        public string Code { get; set; }

        #endregion
    }
}