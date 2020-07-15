using SqlSugar;

namespace Moz.Bus.Models.AdminMenus
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("tab_role_menu")]
    public class RoleMenu : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "menu_id")]
        public long MenuId { get; set; }

        #endregion
    }
}