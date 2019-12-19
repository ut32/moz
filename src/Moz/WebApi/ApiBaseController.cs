using Microsoft.AspNetCore.Mvc;

namespace Moz.WebApi
{
    //[ExceptionHandler(typeof(MyExceptionHandler))] 
    //[ValidationFilter]
    [ApiController]
    [Route("[controller]")]
    public class ApiBaseController : ControllerBase
    {
        protected ApiErrorResult ApiError(string message = "发生错误",int code = 500)
        {
            return new ApiErrorResult(message,code); 
        }

        protected ApiOkResult ApiOk()
        {
            return new ApiOkResult();
        }
    }
}