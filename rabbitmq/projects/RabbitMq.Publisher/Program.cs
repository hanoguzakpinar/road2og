using System.Text;
using RabbitMq.Common;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("logs-direct", ExchangeType.Direct, durable: true);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(async x =>
{
    var routingKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";

    await channel.QueueDeclareAsync(queue: queueName, true, false, false);
    await channel.QueueBindAsync(queueName, "logs-direct", routingKey);
});

Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    LogNames log = (LogNames)new Random().Next(1, 5);

    string msg = $"log-type: {log} log-msg:{x}";
    var body = Encoding.UTF8.GetBytes(msg);

    var routingKey = $"route-{log}";

    await channel.BasicPublishAsync(exchange: "logs-direct", routingKey: routingKey, basicProperties: new BasicProperties(), body: body, mandatory: false);
    System.Console.WriteLine($"Log gönderilmiştir : {log}");
});

Console.ReadLine();