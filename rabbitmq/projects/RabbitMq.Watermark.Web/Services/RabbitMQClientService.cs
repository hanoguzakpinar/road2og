using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace RabbitMq.Watermark.Web.Services;

public class RabbitMQClientService : IAsyncDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IChannel _channel;
    public static string ExchangeName = "ImageDirectExchange";
    public static string RoutingWatermark = "watermark-route-image";
    public static string QueueName = "queue-watermark-image";
    private readonly ILogger<RabbitMQClientService> _logger;
    public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        Connect();
    }

    public async Task<IChannel> Connect()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();

        if (_channel is { IsOpen: true })
            return _channel;

        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, true, false);

        await _channel.QueueDeclareAsync(QueueName, true, false, false, null);

        await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingWatermark);

        _logger.LogInformation("RabbitMq ile bağlantı kuruldu.");

        return _channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        _logger.LogInformation("RabbitMq ile bağlantı kapatıldı.");
    }
}
