using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moz.Bus.Dtos;
using Moz.Core;
using Moz.Core.Options;
using Newtonsoft.Json;

namespace Moz.Exceptions
{
    public abstract class AbstractExceptionHandler<T> : IExceptionHandler
    {
        private readonly ILogger<T> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ExceptionResult _exceptionResult;

        protected AbstractExceptionHandler(ILogger<T> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _exceptionResult = new ExceptionResult();
            switch (exception)
            {
                case AlertException alertException:
                    _exceptionResult.Code = alertException.ErrorCode;
                    _exceptionResult.Message = alertException.Message;
                    break;
                case FatalException fatalException:
                    _exceptionResult.Code = fatalException.ErrorCode;
                    _exceptionResult.Message = fatalException.Message;
                    _logger.LogError("致命错误", exception);
                    break;
                default:
                    _exceptionResult.Code = 20000;
                    _exceptionResult.Message = exception.Message;
                    _logger.LogError("系统错误", exception);
                    break;
            }

            await this.OnExceptionAsync(context, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual async Task OnExceptionAsync(HttpContext context, Exception exception)
        {
            if (IsApiCall(context))
            {
                await OnApiCallAsync(context, exception);
            }
            else
            {
                await OnWebCallAsync(context, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected virtual bool IsApiCall(HttpContext httpContext)
        {
            var isAjaxRequest = httpContext.Request.IsAjaxRequest();
            if (isAjaxRequest)
                return true;

            var isAcceptJson = httpContext.Request.Headers["Accept"]
                                   .ToString()?
                                   .Contains("application/json", StringComparison.OrdinalIgnoreCase) ?? false;
            if (isAcceptJson)
                return true;

            var isApiController =
                httpContext.GetEndpoint()?.Metadata?.GetOrderedMetadata<ApiControllerAttribute>()?.Any() ?? false;
            if (isApiController)
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual async Task OnApiCallAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(_exceptionResult));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual async Task OnWebCallAsync(HttpContext context, Exception exception)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                throw exception;
            }
            var options = EngineContext.Current.Resolve<IOptions<MozOptions>>()?.Value;
            var pathFormat = options?.ErrorPage?.DefaultRedirect;
            if (string.IsNullOrEmpty(pathFormat))
            {
                context.Response.ContentType = "text/html;charset=utf-8";
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync($"错误:{_exceptionResult.Message}({_exceptionResult.Code})");
            }
            else
            {
                //context.Response.StatusCode = 200;
                context.Response.Redirect(string.Format(CultureInfo.InvariantCulture,pathFormat,500)); 
            }
        }
    }
}