using System;
using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     member_role
    /// </summary>
    [SugarTable("tab_member_role")]
    public class MemberRole : BaseModel
    {
        #region 属性

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "member_id")]
        public long MemberId { get; set; }

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