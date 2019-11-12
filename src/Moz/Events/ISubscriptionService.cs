using System.Collections.Generic;

namespace Moz.Events
{
    /// <summary>
    ///     Event subscription service
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        ///     Get subscriptions
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Event consumers</returns>
        IList<ISubscriber<T>> GetSubscriptions<T>();
    }
}