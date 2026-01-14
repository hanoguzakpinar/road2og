using System.Text;
using System.Text.Json;
using RabbitMq.Common;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("header-exchange", ExchangeType.Headers, durable: true);

var headers = new Dictionary<string, object?>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");

var props = new BasicProperties()
{
    Headers = headers,
    Persistent = true // mesajları kalıcı hale getirme
};

var product = new Product { Id = 1, Name = "Kitap", Price = 150, Stock = 100 };

var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(product));

await channel.BasicPublishAsync(exchange: "header-exchange", routingKey: string.Empty, basicProperties: props, body: body, mandatory: false);
System.Console.WriteLine($"Nesne gönderilmiştir : {product.Id}");

Console.ReadLine();