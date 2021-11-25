namespace Moz.Bus.Dtos
{
    public class ErrorResult:PublicResult
    {
        public ErrorResult(string message = "发生错误",int code = 600)
        {
            this.Code = code;
            this.Message = message;  
        }
    }
}