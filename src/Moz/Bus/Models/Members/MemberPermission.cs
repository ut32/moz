using Moz.Bus.Models;
using SqlSugar;

namespace Moz.CMS.Models.Members
{
    /// <summary>
    ///     member_role
    /// </summary>
    [SugarTable("tab_member_permission")]
    public class MemberPermission : BaseModel
    {
        #region 属性 

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "member_id")]
        public long MemberId { get; set; }

        /// <summary>
        /// </summary>
        [SugarColumn(ColumnName = "permission_id")]
        public long PermissionId { get; set; }

        #endregion
    }
}