using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moz.Bus.Dtos;
using Moz.Exceptions;
using Newtonsoft.Json;

namespace Moz.Admin.Layui.Common
{
    public class AdminExceptionHandler : IExceptionHandler
    {
        public Task HandleExceptionAsync(RequestDelegate next, HttpContext context, Exception exception)
        {
            if (context.Request.IsAjaxRequest())
            {
                var response = new BaseRespData();
                if (exception is MozException globalException)
                {
                    response.Code = globalException.ErrorCode;
                    response.Message = globalException.Message;
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

                var result = JsonConvert.SerializeObject(new {response.Code, response.Message});
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 200;
                return context.Response.WriteAsync(result);
            }
            return next(context);
        }
    }
}