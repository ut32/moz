using System.Collections.Generic;

namespace Moz.Core.Config
{
    public class ErrorPageConfig
    { 
        /// <summary>
        /// 默认跳转
        /// </summary>
        public string DefaultRedirect { get; set; }
        
        public List<HttpError> HttpErrors { get; set; } 
    } 
}