using System;
using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     member_role
    /// </summary>
    [SugarTable("tab_user_role")]
    public class UserRole : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "role_id")]
        public long RoleId { get; set; } 

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "expire_date")]
        public DateTime? ExpireDate { get; set; }

        #endregion 
    }
}