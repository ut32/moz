using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Moz.DataBase.Redis
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            } 

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            
            services.Configure(setupAction);
            services.TryAddSingleton<IRedisService,RedisService>();
            
            return services;
        }
    }
}