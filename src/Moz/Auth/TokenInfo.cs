using System;

namespace Moz.Auth
{
    public class TokenInfo
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string JwtToken { get; set; }
        
        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }
        
        /// <summary>
        /// 过期日期
        /// </summary>
        public long ExpireDateTime { get; set; }
        
    }
}