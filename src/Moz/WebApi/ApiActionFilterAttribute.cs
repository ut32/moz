using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moz.Exceptions;

namespace Moz.WebApi
{
    public class ApiActionFilterAttribute:ActionFilterAttribute
    {
        private readonly ILogger<ApiActionFilterAttribute> _logger;
        public ApiActionFilterAttribute(ILogger<ApiActionFilterAttribute> logger)
        {
            _logger = logger;
        }
        
        

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception == null)
            {
                if (context.Result is ObjectResult result)
                {
                    context.Result = new JsonResult(new {
                        Code = 0,
                        Message = "",
                        Data = result.Value
                    });
                }else if (context.Result is OkResult)
                {
                    context.Result = new JsonResult(new
                    {
                        Code = 0,
                        Message = "",
                    });
                }
            }
            else
            {
                var errorCode = 600;
                string errorMessage;
                switch (exception)
                {
                    case AlertException alertException:
                        errorCode = alertException.ErrorCode;
                        errorMessage = alertException.Message;
                        break;
                    case FatalException fatalException:
                        errorCode = fatalException.ErrorCode;
                        errorMessage = fatalException.Message;
                        _logger.LogError(errorMessage,exception);
                        break;
                    default:
                        errorCode = 20000;
                        errorMessage = context.Exception.Message;
                        _logger.LogError(errorMessage,exception);
                        break;
                }
                
                context.Result = new JsonResult(new
                {
                    Code = errorCode,
                    Message = errorMessage
                });
                context.ExceptionHandled = true;
            }
            base.OnActionExecuted(context);
        }
        
    }
}