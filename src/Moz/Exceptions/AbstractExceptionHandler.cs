using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moz.Bus.Dtos;
using Newtonsoft.Json;

namespace Moz.Exceptions
{
    public abstract class AbstractExceptionHandler<T> : IExceptionHandler
    {
        private readonly ILogger<T> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        protected AbstractExceptionHandler(ILogger<T> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var res = new ExceptionResult();
            switch (exception)
            {
                case AlertException alertException:
                    res.Code = alertException.ErrorCode;
                    res.Message = alertException.Message;
                    break;
                case FatalException fatalException:
                    res.Code = fatalException.ErrorCode;
                    res.Message = fatalException.Message;
                    _logger.LogError("致命错误", exception);
                    break;
                case MozAspectInvocationException aspectInvocationException:
                    res.Code = aspectInvocationException.ErrorCode;
                    res.Message = aspectInvocationException.ErrorMessage;
                    break;
                case MozException mozException:
                    res.Code = mozException.ErrorCode;
                    res.Message = mozException.Message;
                    break;
                default:
                    res.Code = 20000;
                    res.Message = exception.Message;
                    _logger.LogError("系统错误", exception);
                    break;
            }
            await this.OnExceptionAsync(context, exception, res);
        }

        protected virtual async Task OnExceptionAsync(HttpContext context, Exception exception, ExceptionResult result)
        {
            var isAjaxRequest = context.Request.IsAjaxRequest();
            var isAcceptJson = context.Request.Headers["Accept"]
                                   .ToString()?
                                   .Contains("application/json", StringComparison.OrdinalIgnoreCase) ?? false;
            var isApiController = context.GetEndpoint().Metadata.GetOrderedMetadata<ApiControllerAttribute>().Any();

            if (isAjaxRequest || isAcceptJson || isApiController)
            {
                context.Response.ContentType = "application/json;charset=utf-8";
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }
            else
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    throw exception;
                }
                context.Response.ContentType = "text/html;charset=utf-8";
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync($"错误:{result.Message}({result.Code})");
            }
        }
    }
}