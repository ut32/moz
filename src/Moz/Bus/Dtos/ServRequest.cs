namespace Moz.Bus.Dtos
{
    public class ServRequest<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public string RequestId { get; set; }
        
        public bool IsValidated { get; set; } 
        public T Data { get; set; } 
        
        public static implicit operator ServRequest<T>(T value)
        {
            return new ServRequest<T>
            {
                Data = value
            };
        }
    }
}