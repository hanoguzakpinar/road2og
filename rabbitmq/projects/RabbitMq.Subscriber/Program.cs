using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

// bu kuyruk geçicidir, consumer uygulama kapandığında kuyruk silinir.
var randomQueueName = (await channel.QueueDeclareAsync()).QueueName;

await channel.QueueBindAsync(queue: randomQueueName, exchange: "logs-fanout", routingKey: string.Empty, arguments: null);

await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new AsyncEventingBasicConsumer(channel);

await channel.BasicConsumeAsync(randomQueueName, autoAck: false, consumer);

System.Console.WriteLine("loglar dinleniyor");

consumer.ReceivedAsync += async (sender, ea) =>
{
    var msg = Encoding.UTF8.GetString(ea.Body.ToArray());

    System.Console.WriteLine($"Gelen Mesaj: {msg}");

    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

    Thread.Sleep(1000);
};

Console.ReadLine();