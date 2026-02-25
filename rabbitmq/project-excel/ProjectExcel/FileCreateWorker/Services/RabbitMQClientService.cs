using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCreateWorker.Services
{
    public class RabbitMQClientService : IAsyncDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IChannel _channel;
        public static string QueueName = "queue-excel-file";
        private readonly ILogger<RabbitMQClientService> _logger;
        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<IChannel> ConnectAsync()
        {
            _connection = await _connectionFactory.CreateConnectionAsync();

            if (_channel is { IsOpen: true })
                return _channel;

            _channel = await _connection.CreateChannelAsync();

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
}
