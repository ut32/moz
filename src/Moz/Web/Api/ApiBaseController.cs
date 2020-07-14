using Microsoft.AspNetCore.Mvc;
using Moz.Exceptions;
using Moz.Validation;

namespace Moz.Web.Api
{
    //[ExceptionHandler(typeof(MyExceptionHandler))] 
    //[ValidationFilter]
    //[ServiceFilter]
    [ApiController]
    [ValidationFilter]
    [TypeFilter(typeof(ApiActionFilterAttribute))]
    [Route("[controller]")]
    public class ApiBaseController : ControllerBase
    {
        /// <summary>
        /// 常规错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected AlertException Alert(string message)
        {
            return new AlertException(message);
        }
        
        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected FatalException Fatal(string message)
        {
            return new FatalException(message);
        }
    }
}