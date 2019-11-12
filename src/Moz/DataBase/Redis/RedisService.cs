using System;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Moz.DataBase.Redis
{
    public class RedisService:IRedisService
    {
        private readonly RedisOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        public RedisService(IOptions<RedisOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _options = optionsAccessor.Value;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        public ConnectionMultiplexer ConnectionMultiplexer => _connectionMultiplexer.Value;

        public IDatabase GetDatabase(int db = -1)
        {
            return _connectionMultiplexer.Value.GetDatabase(db); 
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return _options.ConfigurationOptions != null
                ? ConnectionMultiplexer.Connect(_options.ConfigurationOptions) :
                ConnectionMultiplexer.Connect(_options.Configuration);
        }
    }
}