using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moz.Bus.Dtos;
using Moz.Core;
using Moz.Core.Options;
using Newtonsoft.Json;

namespace Moz.Exceptions
{
    public abstract class AbstractStatusCodePageHandler:IStatusCodePageHandler
    {
        public async Task Process(StatusCodeContext context)
        {
            var statusCode = context.HttpContext.Response.StatusCode;
            if (IsApiCall(context.HttpContext))
            {
                await OnApiCallAsync(context, statusCode);
            }
            else
            {
                await OnWebCallAsync(context, statusCode);
            } 
        } 
        

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

            var isApiController = httpContext.GetEndpoint()?.Metadata?.GetOrderedMetadata<ApiControllerAttribute>()?.Any() ?? false;
            if (isApiController)
                return true;

            return false;
        }

        protected virtual async Task OnApiCallAsync(StatusCodeContext context, int statusCode)
        {
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            context.HttpContext.Response.StatusCode = 200;
            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Code = statusCode,
                Message = ""
            }));
        }

        protected virtual async Task OnWebCallAsync(StatusCodeContext context, int statusCode)
        {
            var options = EngineContext.Current.Resolve<IOptions<MozOptions>>()?.Value;
            var pathFormat = options?.ErrorPage?.DefaultRedirect;
            if (statusCode == 401 && !string.IsNullOrEmpty(options?.ErrorPage?.LoginRedirect))
                pathFormat = options?.ErrorPage?.LoginRedirect;
            if(statusCode == 404&& !string.IsNullOrEmpty(options?.ErrorPage?.NotFoundRedirect))
                pathFormat = options?.ErrorPage?.NotFoundRedirect;
            
            if (string.IsNullOrEmpty(pathFormat))
            {
                context.HttpContext.Response.ContentType = "text/html;charset=utf-8";
                context.HttpContext.Response.StatusCode = 200;
                await context.HttpContext.Response.WriteAsync($"哦豁！系统不想理你，并扔了一个 {statusCode} 页面给你。");
            }
            else
            {
                var newPath = new PathString(string.Format(CultureInfo.InvariantCulture, pathFormat, context.HttpContext.Response.StatusCode));

                var originalPath = context.HttpContext.Request.Path;
                var originalQueryString = context.HttpContext.Request.QueryString;
                // Store the original paths so the app can check it.
                context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
                {
                    OriginalPathBase = context.HttpContext.Request.PathBase.Value,
                    OriginalPath = originalPath.Value,
                    OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null,
                });

                // An endpoint may have already been set. Since we're going to re-invoke the middleware pipeline we need to reset
                // the endpoint and route values to ensure things are re-calculated.
                context.HttpContext.SetEndpoint(endpoint: null);
                var routeValuesFeature = context.HttpContext.Features.Get<IRouteValuesFeature>();
                routeValuesFeature?.RouteValues?.Clear();

                context.HttpContext.Request.Path = newPath;
                //context.HttpContext.Request.QueryString = newQueryString;
                try
                {
                    await context.Next(context.HttpContext);
                }
                finally
                {
                    context.HttpContext.Request.QueryString = originalQueryString;
                    context.HttpContext.Request.Path = originalPath;
                    context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null);
                }
            }
        }
    }
}