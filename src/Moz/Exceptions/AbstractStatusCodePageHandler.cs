using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moz.Bus.Dtos;
using Newtonsoft.Json;

namespace Moz.Exceptions
{
    public abstract class AbstractStatusCodePageHandler:IStatusCodePageHandler
    {
        public async Task Process(HttpContext context)
        {
            var code = context.Response.StatusCode;
            var isAjaxRequest = context.Request.IsAjaxRequest();
            var isAcceptJson = context.Request.Headers["Accept"]
                                   .ToString()?
                                   .Contains("application/json", StringComparison.OrdinalIgnoreCase) ?? false;

            if (isAjaxRequest || isAcceptJson)
            {
                await this.OnApiCallAsync(context, code);
            }
            else
            {
                await this.OnPageCallAsync(context, code);
            }
        }

        protected virtual async Task OnApiCallAsync(HttpContext context, int statusCode)
        {
            var res = new ServResult
            {
                Code = statusCode,
                Message = $"哦豁！系统不想理你，并扔了一个 {statusCode} 页面给你。"
            };
            var result = JsonConvert.SerializeObject(res);
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(result);
        }

        protected virtual async Task OnPageCallAsync(HttpContext context, int statusCode)
        {
            context.Response.ContentType = "text/html;charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync($"哦豁！系统不想理你，并扔了一个 {statusCode} 页面给你。");
        }
    }
}