namespace Moz.Core.Options
{
    public class ErrorPageOptions
    { 
        /// <summary>
        /// 登录页，遇401将跳转
        /// </summary>
        public string LoginRedirect { get; set; }
        
        /// <summary>
        /// 404页面，遇404将跳转
        /// </summary>
        public string NotFoundRedirect { get; set; }
        
        /// <summary>
        /// 默认跳转页
        /// </summary>
        public string DefaultRedirect { get; set; }  
    } 
}