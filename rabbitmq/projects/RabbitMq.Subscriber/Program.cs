using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("header-exchange", ExchangeType.Headers, durable: true);

await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new AsyncEventingBasicConsumer(channel);

var queueName = (await channel.QueueDeclareAsync()).QueueName;

var headers = new Dictionary<string, object?>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "all");

await channel.QueueBindAsync(queueName, "header-exchange", string.Empty, headers);

await channel.BasicConsumeAsync(queueName, autoAck: false, consumer);

System.Console.WriteLine("loglar dinleniyor");

consumer.ReceivedAsync += async (sender, ea) =>
{
    var msg = Encoding.UTF8.GetString(ea.Body.ToArray());

    System.Console.WriteLine($"Gelen Mesaj: {msg}");

    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

    Thread.Sleep(1000);
};

Console.ReadLine();