namespace Moz.Bus.Dtos
{
    public class ServRequest<T>
    {
        public string RequestId { get; set; }
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