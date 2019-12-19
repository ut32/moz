namespace Moz.WebApi
{
    public class ApiErrorResult:ApiResult
    {
        public ApiErrorResult(string message,int code)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}