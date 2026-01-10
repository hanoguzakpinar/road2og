using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    Uri = new Uri("amqps://hcjxfkdh:U7T630lKSQsPOHo3DnL1H0aG9-Zq1Mqw@collie.lmq.cloudamqp.com/hcjxfkdh")
};

using var connection = await factory.CreateConnectionAsync();

var channel = await connection.CreateChannelAsync();

//kuyruk declare edilmezse ve bu kuyruk yoksa hata fırlatılır.
//publisher'ın bu kuyruğu oluşturduğuna dair kesinlik varsa bu satır silinebilir.
//zaten kuyruk varsa ve bu satır çalışırsa, hata oluşmaz.
//publisher tarafında ve consumer tarafında aynı parametreler ile kuyruk oluşturulmalıdır. yoksa uygulama hata verir.
//await channel.QueueDeclareAsync("hello-queue", true, false, false);

await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new AsyncEventingBasicConsumer(channel);

await channel.BasicConsumeAsync("work-queue", autoAck: false, consumer);

consumer.ReceivedAsync += async (sender, ea) =>
{
    var msg = Encoding.UTF8.GetString(ea.Body.ToArray());

    System.Console.WriteLine($"Gelen Mesaj: {msg}");

    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

    Thread.Sleep(1500);
};


Console.ReadLine();