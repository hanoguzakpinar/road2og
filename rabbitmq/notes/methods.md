# QueueDeclareAsync
**durable** = RabbitMQ sunucusu yeniden başlatılsa (restart) bile kuyruğun veya exchange'in kaybolmayıp fiziksel olarak diskte saklanmasını sağlar.
**exclusive** = Kuyruğun sadece onu oluşturan bağlantı (connection) tarafından kullanılabilmesini ve o bağlantı kapandığında kuyruğun otomatik olarak silinmesini sağlar.
**autoDelete** = Kuyruğu dinleyen son tüketici (consumer) aboneliğini (unsubscribe) bitirdiğinde kuyruğun otomatik olarak silinmesini sağlar.

# BasicConsumeAsync
**autoAck** = Mesaj tüketiciye ulaştığı anda, tüketici tarafındaki işlemin bitmesi veya hata alması beklenmeden mesajın başarılı sayılmasını ve kuyruktan hemen silinmesini sağlar.