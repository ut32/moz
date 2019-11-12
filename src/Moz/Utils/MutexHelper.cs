using System;
using System.Collections.Concurrent;

namespace Moz.Utils
{
    public class MutexHelper
    {
        private static readonly object ObjectLock = new object();
        private static MutexHelper _mutexHelper;

        private readonly ConcurrentDictionary<string, object> _mutexDict = new ConcurrentDictionary<string, object>();

        private MutexHelper()
        {
        }

        public static MutexHelper Instance
        {
            get
            {
                if (_mutexHelper == null)
                    lock (ObjectLock)
                    {
                        if (_mutexHelper == null) _mutexHelper = new MutexHelper();
                    }

                return _mutexHelper;
            }
        }

        public object GetOrAdd(string id, Func<object> func)
        {
            if (_mutexDict.ContainsKey(id))
                return _mutexDict[id];
            var obj = func();
            _mutexDict[id] = obj;
            return obj;
        }
    }
}