namespace Moz.Bus.Dtos
{
    
    public class ServResult 
    {
        public int Code { get; set; }
        public string Message { get; set; }
    } 
    
    public class ServResult<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static implicit operator ServResult<T>(T value)
        {
            return new ServResult<T>
            {
                Data = value
            };
        }

        public static implicit operator ServResult<T>(ServResult value)
        {
            return new ServResult<T>
            {
                Code = value.Code,
                Message = value.Message
            };
        }
    }
}