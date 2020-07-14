namespace Moz.Core.Config
{
    public class HttpError
    {

        public HttpError(int statusCode, string path, ResponseMode mode)
        {
            StatusCode = statusCode;
            Path = path;
            Mode = mode;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; } 
        
        /// <summary>
        /// 执行/跳转地址
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 模式
        /// </summary>
        public ResponseMode Mode { get; set; } 
            
    }
    public enum ResponseMode
    {
        /// <summary>
        /// 地址不跳转
        /// </summary>
        Execute,
        
        /// <summary>
        /// 跳转
        /// </summary>
        Redirect 
    }
}