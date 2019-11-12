using System;
using System.IO;

namespace Moz.Utils
{
    public class HttpHelper
    {
        public static string MapPath(string baseDirectory, string path)
        {
            if (string.IsNullOrEmpty(baseDirectory) || string.IsNullOrEmpty(path))
                throw new Exception("the path is invalid");
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory ?? string.Empty, path);
        }

        public static string UrlCombine(string baseUrl, string url)
        {
            baseUrl = baseUrl?.TrimEnd('/');
            url = url?.TrimStart('/');
            return $"{baseUrl}/{url}";
        }
    }
}