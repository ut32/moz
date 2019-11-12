using System;
using System.Collections.Generic;

namespace Moz.Core
{
    public interface IEngine
    {
        T Resolve<T>() where T : class;
        object Resolve(Type type);
        IEnumerable<object> ResolveAll(Type type);
        IEnumerable<T> ResolveAll<T>();
        object ResolveUnregistered(Type type);
    }
}