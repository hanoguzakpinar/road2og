using System.Text;
using RabbitMq.Common;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("logs-topic", ExchangeType.Topic, durable: true);

Random rnd = new Random();
Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    string msg = $"log-type: {log1}.{log2}.{log3}";
    var body = Encoding.UTF8.GetBytes(msg);

    var routingKey = $"{log1}.{log2}.{log3}";

    await channel.BasicPublishAsync(exchange: "logs-topic", routingKey: routingKey, basicProperties: new BasicProperties(), body: body, mandatory: false);
    System.Console.WriteLine($"Log gönderilmiştir : {msg}");
});

Console.ReadLine();