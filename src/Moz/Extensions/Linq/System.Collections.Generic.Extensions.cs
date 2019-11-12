using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class SystemCollectionsGenericExtensions
    {
        public static string Join<T>(this IEnumerable<T> value,string separator)
        {
            return value == null ? null : string.Join(separator, value);
        }
        
        public static string Join<T>(this IEnumerable<T> value,char separator)
        {
            return value == null ? null : string.Join(separator, value);
        }
    }
}