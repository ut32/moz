namespace Moz.WebApi
{
    public class ApiErrorResult:ApiResult
    {
        public ApiErrorResult(string message = "发生错误",int code = 600)
        {
            this.Code = code;
            this.Message = message; 
        }
    }
}