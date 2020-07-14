using System;

namespace Moz.Common
{
    
    public class GenericCache<T>
    {
        private GenericCache()
        {
        }

        public static T Instance { get; set; }

        public static T GetOrSet(Func<T> func)
        {
            if (Instance != null)
            {
                return Instance;
            }
            else
            {
                if (func != null)
                    Instance = func();
                return Instance;
            }
        }
    }
}