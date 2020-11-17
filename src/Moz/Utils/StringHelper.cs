using System.Text.RegularExpressions;

namespace Moz.Utils
{
    public static class StringHelper
    {
        public static string RemoveHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) 
                return html;
            
            html = Regex.Replace(html, @"(\r\n)+|\r+|\n+|\t+", "");
            html = Regex.Replace(html, @"<[^>]*>", "");
            html = Regex.Replace(html, @"&[a-zA-Z]+;", "");
            html = Regex.Replace(html, @"\s{2,}", " ");
            
            return html.Trim();
        }
    }
}