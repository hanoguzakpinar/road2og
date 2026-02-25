using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace ProjectExcel.Mvc.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitService;

        public RabbitMQPublisher(RabbitMQClientService rabbitService)
        {
            _rabbitService = rabbitService;
        }

        public async Task PublishAsync(CreateExcelMessage message)
        {
            var channel = await _rabbitService.ConnectAsync();

            var bodyJson = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(bodyJson);

            var properties = new BasicProperties()
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingExcel, body: body, basicProperties: properties, mandatory: false);
        }
    }
}
