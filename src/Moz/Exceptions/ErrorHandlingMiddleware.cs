using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moz.Bus.Dtos;
using Moz.Exceptions;
using Newtonsoft.Json;

namespace Moz.Exceptions
{
    public class ErrorHandlingMiddleware:AbstractExceptionHandler<ErrorHandlingMiddleware>
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
            :base(logger,webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Invoke(HttpContext context)
        {
            ExceptionDispatchInfo edi;
            try
            {
                var task = _next(context);
                if (!task.IsCompletedSuccessfully)
                {
                    return Awaited(this, context, task);
                }
                return Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }
            return HandleException(context, edi);

            static async Task Awaited(ErrorHandlingMiddleware middleware, HttpContext context, Task task)
            {
                ExceptionDispatchInfo edi = null;
                try
                {
                    await task;
                }
                catch (Exception exception)
                {
                    edi = ExceptionDispatchInfo.Capture(exception);
                }

                if (edi != null)
                {
                    await middleware.HandleException(context, edi);
                }
            }
        }
        
        private async Task HandleException(HttpContext context, ExceptionDispatchInfo edi)
        {
            if (context.Response.HasStarted)
            {
                edi.Throw();
            }
            try
            {
                context.Response.Clear();
                context.Response.OnStarting(ClearCacheHeaders, context.Response);

                var endpoint = context.GetEndpoint();
                var exceptionHandlerAttribute = endpoint.Metadata.GetOrderedMetadata<ExceptionHandlerAttribute>().FirstOrDefault();
                if (exceptionHandlerAttribute != null
                    && _serviceProvider.GetService(exceptionHandlerAttribute.ExceptionHandlerType) is IExceptionHandler exceptionHandler)
                {
                    await exceptionHandler.HandleExceptionAsync(context, edi.SourceException);
                }
                else
                {
                    await base.HandleExceptionAsync(context, edi.SourceException);
                }
                return;
            }
            catch (Exception ex2)
            {
                // ignored
            }
            edi.Throw();
        }

        private static Task ClearCacheHeaders(object state)
        {
            var headers = ((HttpResponse)state).Headers;
            headers[HeaderNames.CacheControl] = "no-cache";
            headers[HeaderNames.Pragma] = "no-cache";
            headers[HeaderNames.Expires] = "-1";
            headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}