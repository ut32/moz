using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     role_permisson
    /// </summary>
    [SugarTable("tab_role_permisson")]
    public class RolePermisson : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "permisson_id")]
        public long PermissonId { get; set; }

        #endregion
    }
}