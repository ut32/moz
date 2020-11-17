using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Moz.Utils
{
    public class HttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// </summary>
        /// <param name="tryUseXForwardHeader"></param>
        /// <returns></returns>
        public string GetRequestIp(bool tryUseXForwardHeader = true)
        {
            var ip = string.Empty;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763

            if (tryUseXForwardHeader)
                ip = SplitCsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            return ip;
        }

        /// <summary>
        /// </summary>
        /// <param name="headerName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetHeaderValueAs<T>(string headerName)
        {
            if (!(_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out var values) ?? false))
                return default(T);

            //var rawValues = values.ToString();
            //if (!rawValues.IsNullOrEmpty())
            //    return (T) Convert.ChangeType(rawValues, typeof(T));

            return default(T);
        }

        /// <summary>
        /// </summary>
        /// <param name="csvList"></param>
        /// <param name="nullOrWhitespaceInputReturnsNull"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}