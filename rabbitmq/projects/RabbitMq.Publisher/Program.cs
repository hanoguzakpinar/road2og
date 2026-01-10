using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

// durable = RabbitMQ sunucusu yeniden başlatılsa (restart) bile kuyruğun veya exchange'in kaybolmayıp fiziksel olarak diskte saklanmasını sağlar.
// exclusive = Kuyruğun sadece onu oluşturan bağlantı (connection) tarafından kullanılabilmesini ve o bağlantı kapandığında kuyruğun otomatik olarak silinmesini sağlar.
// autoDelete = Kuyruğu dinleyen son tüketici (consumer) aboneliğini (unsubscribe) bitirdiğinde kuyruğun otomatik olarak silinmesini sağlar.
await channel.QueueDeclareAsync("hello-queue", true, false, false);

string msg = "hello world";
var body = Encoding.UTF8.GetBytes(msg);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello-queue", basicProperties: new BasicProperties(), body: body, mandatory: false);

System.Console.WriteLine("Mesaj gönderilmiştir.");
Console.ReadLine();