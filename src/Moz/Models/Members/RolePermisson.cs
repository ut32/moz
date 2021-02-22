using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     
    /// </summary>
    [SugarTable("tab_role_permission")]
    public class RolePermission : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "permission_id")]
        public long PermissionId { get; set; }

        #endregion
    }
}