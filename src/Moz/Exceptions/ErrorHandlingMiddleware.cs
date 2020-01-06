using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                var isExceptionHandled = await HandleExceptionAsync(context, ex);
                if(isExceptionHandled == IsExceptionHandled.No)
                   throw;
            }
        }

        private new async Task<IsExceptionHandled> HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            var endpoint = context.GetEndpoint();
            var exceptionHandlerAttribute = endpoint.Metadata.GetOrderedMetadata<ExceptionHandlerAttribute>().FirstOrDefault();
            if (exceptionHandlerAttribute != null
                && _serviceProvider.GetService(exceptionHandlerAttribute.ExceptionHandlerType) is IExceptionHandler exceptionHandler)
            {
                return await exceptionHandler.HandleExceptionAsync(context, exception);
            }
            else
            {
                return await base.HandleExceptionAsync(context, exception);
            }
        }
    }
}