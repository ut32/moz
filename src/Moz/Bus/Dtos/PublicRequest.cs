namespace Moz.Bus.Dtos
{
    public class PublicRequest
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeStamp { get; set; }
        
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }
    
    public class PublicRequest<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; } 
        
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeStamp { get; set; }
        
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }
}