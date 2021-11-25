using System;
using Moz.Bus.Models;
using Moz.Bus.Models.Members;
using SqlSugar;

namespace Moz.Model
{
    public enum ExternalAuthProvider
    {
        Weibo = 1,
        Qq = 2,
        WxMiniProgram = 3,
        WxMp = 4,
        Google = 5,
        Facebook = 6,
        Twitter = 7,
        Github = 8,
        TencentUnion = 9,
        Guest = 10,
        Other = 99
    }
    
    /// <summary>
    ///  三方登录
    /// </summary>
    [SugarTable("tab_external_auth")]
    public class ExternalAuth : BaseModel
    {
        #region 属性 
 
        /// <summary>
        /// 绑定用户
        /// </summary>
        [SugarColumn(ColumnName = "user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public ExternalAuthProvider Provider { get; set; }

        /// <summary>
        /// open id
        /// </summary>
        public string OpenId { get; set; } 

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