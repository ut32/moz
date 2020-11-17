using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Moz.Core;
using Moz.Utils;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class TypeExtension
    {
        public static bool IsAnonymousType(this Type type)
        {
            return string.IsNullOrEmpty(type.Namespace);
        }
    }

    public static class StringExtension
    {
        public static bool IsNumbers(this string value)
        {
            return !value.IsNullOrEmpty() && int.TryParse(value, out _);
        }

        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        
        /// <summary>
        /// Validates a URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string url) 
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var validatedUri))
            {
                if ("0.0.0.0".Equals(validatedUri.Host)) return false;
                return validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFullPath(this string url)
        {
            if (url.IsNullOrEmpty()) return "";
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                return url;
            var host = EngineContext.Current.Resolve<IConfiguration>()["StaticHost"];
            if (host.IsNullOrEmpty()) return url;
            else
            {
                return host + url;
            }
        }
        
        /// <summary>
        /// 获取截取字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubstring(this string value, int length)
        {
            if (value.IsNullOrEmpty()) return null;
            return value.Length <= length ? value : value.Substring(0, length) + "...";
        }

        public static string IfEmptyReturn(this string value, string defaultValue)
        {
            return value.IsNullOrEmpty() ? defaultValue : value; 
        }

        /// <summary>
        /// 移除HTML标签
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : StringHelper.RemoveHtml(value);
        }
    }
}