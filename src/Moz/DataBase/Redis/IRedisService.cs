using Moz.Core.Attributes;
using StackExchange.Redis;

namespace Moz.DataBase.Redis
{
    
    [IgnoreRegister]
    public interface IRedisService
    {
        IDatabase GetDatabase(int db = -1);
        ConnectionMultiplexer ConnectionMultiplexer { get; }
    }
}