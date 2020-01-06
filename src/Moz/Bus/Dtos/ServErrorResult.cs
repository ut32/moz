namespace Moz.Bus.Dtos
{
    public class ServErrorResult:ServResult
    {
        public ServErrorResult(string message = "发生错误",int code = 600)
        {
            this.Code = code;
            this.Message = message;  
        }
    }
}