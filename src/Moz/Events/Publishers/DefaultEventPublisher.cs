using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Moz.Events.Publishers
{  
    public sealed class DefaultEventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger<DefaultEventPublisher> _logger;

        public DefaultEventPublisher(ISubscriptionService subscriptionService,
                                     ILogger<DefaultEventPublisher> logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }

        public void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();
            var tasks = subscriptions
                .Select(t => Task.Run(() => PublishToSubscriber(t, eventMessage)))
                .ToList();
        }

        private void PublishToSubscriber<T>(ISubscriber<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                _logger.LogError("发布订阅执行出错/", exc.Message, DateTime.Now.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}