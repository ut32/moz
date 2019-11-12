using System;
using System.Collections.Generic;
using System.Linq;

namespace Moz.Utils
{
    public static class BaseHelper
    {
        private static readonly List<char> Chars = Enumerable.Range(48, 10)
            .Union(Enumerable.Range(65, 26))
            .Union(Enumerable.Range(97, 26))
            .Select(t => (char)t)
            .ToList();
        
        public static string ToAnyBase(long num, int @base)
        {
            var temp = num;
            var result = "";
            while (temp != 0)
            {
                var mod = temp % @base;
                temp = temp / @base;
                result = Chars[(int)mod] + result;
            }
            return result;
        }
        
        
        public static long FromAnyBase(string str, int @base)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            return (long)str.Reverse().Select((s, i) => (Chars.IndexOf(s)) * Math.Pow(@base, i)).Sum();
        }
    }
}