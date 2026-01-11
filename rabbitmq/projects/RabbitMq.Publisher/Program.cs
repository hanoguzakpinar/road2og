using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("logs-fanout", ExchangeType.Fanout, durable: true);

Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    string msg = $"log {x}";
    var body = Encoding.UTF8.GetBytes(msg);

    await channel.BasicPublishAsync(exchange: "logs-fanout", routingKey: string.Empty, basicProperties: new BasicProperties(), body: body, mandatory: false);
    System.Console.WriteLine($"Mesaj gönderilmiştir : {x}");
});

Console.ReadLine();