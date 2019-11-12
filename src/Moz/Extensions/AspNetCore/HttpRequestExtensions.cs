using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request?.Headers != null 
                   && "XMLHttpRequest".Equals(request.Headers["X-Requested-With"], StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsJwtRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null && !string.IsNullOrEmpty(request.Headers["Authorization"]))
            {
                var auth = request.Headers["Authorization"].ToString();
                return auth.StartsWith("bearer ", StringComparison.CurrentCultureIgnoreCase)
                       || auth.StartsWith("basic ", StringComparison.CurrentCultureIgnoreCase);
            }

            return false;
        }
    }
}