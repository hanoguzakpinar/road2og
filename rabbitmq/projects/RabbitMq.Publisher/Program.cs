using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("work-queue", durable: true, exclusive: false, autoDelete: false);

Enumerable.Range(1, 50).ToList().ForEach(async x =>
{
    string msg = $"Message {x}";
    var body = Encoding.UTF8.GetBytes(msg);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "work-queue", basicProperties: new BasicProperties(), body: body, mandatory: false);
    System.Console.WriteLine($"Mesaj gönderilmiştir : {x}");
});

Console.ReadLine();