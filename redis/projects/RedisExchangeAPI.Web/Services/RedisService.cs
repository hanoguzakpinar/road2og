using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services;

public class RedisService
{
    private readonly string _host;
    private readonly string _port;
    public IDatabase db { get; set; }
    private ConnectionMultiplexer _redis;
    public RedisService(IConfiguration configuration)
    {
        _host = configuration["Redis:Host"];
        _port = configuration["Redis:Port"];
    }
    public void Connect()
    {
        var config = $"{_host}:{_port}";

        _redis = ConnectionMultiplexer.Connect(config);
    }
    public IDatabase GetDb(int db)
    {
        return _redis.GetDatabase(db);
    }
}