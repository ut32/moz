using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moz.Bus.Dtos;
using Moz.CMS.Dtos;
using Moz.CMS.Dtos;
using Moz.Exceptions;
using Newtonsoft.Json;

namespace Moz.Exceptions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            //context.Request.
            var handler = context?.GetRouteData()?.Values["custom_exception_handler"];
            if (handler != null && handler is IExceptionHandler exceptionHandler)
                return exceptionHandler.HandleExceptionAsync(next, context, exception);

            if ((context?.Request?.IsAjaxRequest() ?? false)
                || (context?.Request?.Headers["Accept"].ToString()?.ToLower()?.Contains("application/json") ?? false))
            {
                var response = new BaseRespData();
                if (exception is MozException myException)
                {
                    response.Code = myException.ErrorCode;
                    response.Message = myException.Message;
                }
                else if (exception is MozAspectInvocationException aspectInvocationException)
                {
                    response.Code = aspectInvocationException.ErrorCode;
                    response.Message = aspectInvocationException.ErrorMessage;
                }
                else
                {
                    response.Code = 999;
                    response.Message = exception.Message;
                }
                var result = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 200;
                return context.Response.WriteAsync(result);
            }
            return next(context);
        }
    }
}