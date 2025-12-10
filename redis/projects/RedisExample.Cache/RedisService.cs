using StackExchange.Redis;

namespace RedisExample.Cache;

public class RedisService
{
    private readonly ConnectionMultiplexer _connection;
    public RedisService(string url)
    {
        _connection = ConnectionMultiplexer.Connect(url);
    }
    public IDatabase GetDb(int db)
    {
        return _connection.GetDatabase(db);
    }
}