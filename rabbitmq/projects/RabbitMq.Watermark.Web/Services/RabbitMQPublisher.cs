using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace RabbitMq.Watermark.Web.Services;

public class RabbitMQPublisher
{
    private readonly RabbitMQClientService _rabbitService;

    public RabbitMQPublisher(RabbitMQClientService rabbitService)
    {
        _rabbitService = rabbitService;
    }

    public async Task PublishAsync(ProductImageCreatedEvent createdEvent)
    {
        var channel = await _rabbitService.ConnectAsync();

        var bodyJson = JsonSerializer.Serialize(createdEvent);
        var body = Encoding.UTF8.GetBytes(bodyJson);

        var properties = new BasicProperties()
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark, body: body, basicProperties: properties, mandatory: false);
    }
}
