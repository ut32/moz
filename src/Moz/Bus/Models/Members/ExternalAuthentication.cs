using System;
using SqlSugar;

namespace Moz.Bus.Models.Members
{
    /// <summary>
    ///     external_authentication
    /// </summary>
    [SugarTable("tab_external_authentication")]
    public class ExternalAuthentication : BaseModel
    {
        #region 属性

        /// <summary>
        /// 绑定用户
        /// </summary>
        [SugarColumn(ColumnName = "member_id")]
        public long MemberId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public ExternalAuthProvider Provider { get; set; }

        /// <summary>
        /// open id
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// access token
        /// </summary>
        [SugarColumn(ColumnName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        [SugarColumn(ColumnName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        [SugarColumn(ColumnName = "expire_dt")]
        public DateTime? ExpireDt { get; set; }
        
        #endregion 
    }
}