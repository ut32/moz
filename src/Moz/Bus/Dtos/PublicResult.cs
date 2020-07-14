namespace Moz.Bus.Dtos
{
    public class PublicResult 
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
    } 
    
    public class PublicResult<T>
    {
        
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PublicResult<T>(T value)
        {
            return new PublicResult<T>
            {
                Data = value
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PublicResult<T>(PublicResult value)
        {
            return new PublicResult<T>
            {
                Code = value.Code,
                Message = value.Message
            };
        }
    }
}