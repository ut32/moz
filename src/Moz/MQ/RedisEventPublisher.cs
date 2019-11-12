using System;
using System.Linq;
using Moz.DataBase;
using Moz.Events;
using Newtonsoft.Json;

namespace Moz.MQ
{
    public class s
    {
        static s()
        {
            /*
            var timer = new Timer()
            {
                Enabled = true,
                Interval = 1000
            };
            timer.Elapsed += TimerElapsed;
            */
            /*
            RedisConnectionFactory.DefaultSubscriber.Subscribe("redis_event_bug_pub/sub", (channel, value) =>
            {
                var mqMessage = JsonConvert.DeserializeObject<EventMessage>(value);
                var entityType = Type.GetType(mqMessage.Type);
                var interfaceType = typeof(ISubscriber<>);
                Type[] typeArgs = { entityType };
                var makeGenericType = interfaceType.MakeGenericType(typeArgs);
                var allConsumers = EngineContext.Current.ResolveAll(makeGenericType).ToList();
                allConsumers.ForEach(t =>
                {
                    try
                    {
                        dynamic data = JsonConvert.DeserializeObject(mqMessage.Data, entityType);
                        dynamic d = t;
                        d.HandleEvent(data);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                });
            });
            */
        }

        /*
         * 
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var message = RedisConnectionFactory.Connection.GetDatabase().ListLeftPop("mq");
            if (!message.IsNullOrEmpty)
            {
                var mqMessage = JsonConvert.DeserializeObject<MqMessage>(message);
                var entityType = Type.GetType(mqMessage.Type);
                var interfaceType = typeof(IConsumer<>);
                Type[] typeArgs = { entityType };
                var makeGenericType = interfaceType.MakeGenericType(typeArgs);
                var allConsumers = EngineContext.Current.ResolveAll(makeGenericType).ToList();
                allConsumers.ForEach(t =>
                {
                    try
                    {
                        dynamic data = JsonConvert.DeserializeObject(mqMessage.Data, entityType);
                        dynamic d = t;
                        d.HandleEvent(data);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                });
            }
        }
        */

        public void Publish<T>(T eventMessage)
        {     
            var message = JsonConvert.SerializeObject(new EventMessage
            {
                Type = typeof(T).AssemblyQualifiedName,
                Data =  JsonConvert.SerializeObject(eventMessage)
            });
            //RedisConnectionFactory.DefaultSubscriber.Publish("redis_event_bug_pub/sub", message);
            //RedisConnectionFactory.Connection.GetDatabase().ListRightPush("mq", message);    
        }
    }
}