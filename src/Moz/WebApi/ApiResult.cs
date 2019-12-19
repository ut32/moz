namespace Moz.WebApi
{
    
    public class ApiResult 
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
    
    public class ApiResult<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static implicit operator ApiResult<T>(T value)
        {
            return new ApiResult<T>
            {
                Data = value
            };
        }

        public static implicit operator ApiResult<T>(ApiResult value)
        {
            return new ApiResult<T>
            {
                Code = value.Code,
                Message = value.Message
            };
        }
    }
}