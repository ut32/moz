using System.Collections.Generic;
using System.Linq;
using Moz.Core;

namespace Moz.Events
{
    public class SubscriptionService : ISubscriptionService
    {     
        public IList<ISubscriber<T>> GetSubscriptions<T>()
        {
            return EngineContext.Current.ResolveAll<ISubscriber<T>>().ToList();
        }
    }
}