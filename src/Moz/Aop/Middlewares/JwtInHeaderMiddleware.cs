using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moz.Core.Options;
using Moz.Service.Security;

namespace Moz.Aop.Middlewares
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class JwtInHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEncryptionService _encryptionService;
        private readonly IOptions<MozOptions> _options;

        public JwtInHeaderMiddleware(RequestDelegate next,IEncryptionService encryptionService, IOptions<MozOptions> options)
        {
            _next = next;
            _encryptionService = encryptionService;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            const string name = "__moz__token";
            var cookie = context.Request?.Cookies[name];

            if (!string.IsNullOrEmpty(cookie) && !(context.Request?.Headers?.ContainsKey("Authorization") ?? false))
            {
                var key = _options.Value.EncryptKey ?? "gvPXwK50tpE9b6P7";
                try
                {
                    var decryptString = _encryptionService.DecryptText(cookie, key);
                    context.Request?.Headers?.Append("Authorization", "Bearer " + decryptString);
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            await _next.Invoke(context);
        }
    }
}