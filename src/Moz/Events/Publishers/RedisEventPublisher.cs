using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moz.DataBase;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Moz.Events.Publishers
{
    public class RedisEventPublisher:IEventPublisher
    {       
        static RedisEventPublisher()
        {
            /*
            RedisConnectionFactory.DefaultSubscriber.Subscribe("redis_event_pub_sub", (channel, value) =>
            {
                var mqMessage = JsonConvert.DeserializeObject<EventMessage>(value);
                var entityType = Type.GetType(mqMessage.Type);
                var interfaceType = typeof(ISubscriber<>);
                Type[] typeArgs = { entityType };
                var makeGenericType = interfaceType.MakeGenericType(typeArgs);
                var allSubscribers = EngineContext.Current.ResolveAll(makeGenericType).ToList();
                Console.WriteLine("Subscribe Main Method:{0} thread id: {1}",DateTime.Now.Second, Thread.CurrentThread.ManagedThreadId);
                var tasks = allSubscribers.Select(subscriber => Task.Run(() =>
                {
                    try
                    {
                        dynamic data = JsonConvert.DeserializeObject(mqMessage.Data, entityType);
                        dynamic d = subscriber;
                        d.HandleEvent(data);
                    }
                    catch (Exception exception)
                    {
                        var logger = EngineContext.Current.Resolve<ILogger<RedisEventPublisher>>();
                        logger.LogError("发布订阅执行出错/", exception.Message, DateTime.Now.ToString(CultureInfo.InvariantCulture));
                        throw;
                    }
                })).ToList();

            });
            */
        }

        public void Publish<T>(T eventMessage)
        {     
            /*
            var message = JsonConvert.SerializeObject(new EventMessage
            {
                Type = typeof(T).AssemblyQualifiedName,
                Data =  JsonConvert.SerializeObject(eventMessage)
            });
            */
            //RedisConnectionFactory.DefaultSubscriber.Publish("redis_event_pub_sub", message); 
              
        }
    }
}