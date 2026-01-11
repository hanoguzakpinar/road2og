# QueueDeclareAsync
QueueDeclareAsync, RabbitMQ sunucusu üzerinde bir kuyruk (queue) oluşturmak veya var olan bir kuyruğun varlığını garanti altına almak için kullanılan asenkron metottur.

**queue** = Kuyruğun ismi.

**durable** = RabbitMQ sunucusu yeniden başlatılsa (restart) bile kuyruğun veya exchange'in kaybolmayıp fiziksel olarak diskte saklanmasını sağlar. true ise diskte saklanır (güvenli), false ise RAM'de (hızlı ama riskli).

**exclusive** = Kuyruğun sadece onu oluşturan bağlantı (connection) tarafından kullanılabilmesini ve o bağlantı kapandığında kuyruğun otomatik olarak silinmesini sağlar.

**autoDelete** = Kuyruğu dinleyen son tüketici (consumer) aboneliğini (unsubscribe) bitirdiğinde kuyruğun otomatik olarak silinmesini sağlar.

**arguments** = Kuyruğun maksimum boyutu, mesajların yaşam süresini (TTL) vb. belirleyen sözlük (dictionary).

Bu metodun en önemli özelliği "Idempotent" olmasıdır. Yani:
- Kuyruk yoksa: Yeni bir kuyruk oluşturur.
- Kuyruk zaten varsa: Hiçbir şey yapmaz, yoluna devam eder (Hata vermez).
- Kuyruk var ama parametreleri farklıysa: Hata fırlatır (Örneğin; var olan kuyruk durable: true iken siz durable: false ile tekrar oluşturmaya çalışırsanız).

# BasicConsumeAsync
RabbitMQ'ya "Ben buradayım, şu kuyruğu dinlemek istiyorum, mesaj geldikçe bana gönder" demenizi sağlayan, aboneliği (subscription) başlatan asenkron metottur.

**queue** = Hangi kuyruğun dinleneceği.

**autoAck** = Mesaj tüketiciye ulaştığı anda, tüketici tarafındaki işlemin bitmesi veya hata alması beklenmeden mesajın başarılı sayılmasını ve kuyruktan hemen silinmesini sağlar.

**consumer** = Mesaj geldiğinde ReceivedAsync eventi tetiklenecek olan nesne.

# BasicPublishAsync
Bir mesajı RabbitMQ sunucusuna (Exchange'e) göndermek için kullanılan asenkron metottur.

**exchange** = Mesajı kime teslim edeceğiniz (Boş bırakırsanız varsayılan exchange olur).

**routing Key** = Mesajın hangi kuyruğa gideceğini belirleyen anahtar/adres.

**body** = Gönderilecek verinin kendisi (Byte dizisi formatında).

# BasicAckAsync
Bir mesajın tüketici (consumer) tarafından başarıyla işlendiğini ve artık kuyruktan silinebileceğini manuel olarak bildirmek için kullanılır.

**multiple** = 
- false: Sadece bu deliveryTag'e sahip mesajı onayla.
- true: Bu deliveryTag ve ondan önceki tüm onaylanmamış mesajları topluca onayla.


# BasicQos
RabbitMQ'nun tüketiciye (consumer) aynı anda kaç tane işlenmemiş (onaylanmamış) mesaj gönderebileceğini belirleyen ayardır.

Bu ayar, "Yük Dengeleme" (Load Balancing) ve "Adil Dağıtım" (Fair Dispatch) için kritik öneme sahiptir.

**prefetchSize** = Mesajların byte cinsinden toplam boyutunu sınırlar. Genellikle kullanılmaz ve 0 verilir (sınırsız).

**prefetchCount** = Tüketicinin aynı anda üzerinde çalışabileceği maksimum mesaj sayısı.
- 1 yaparsanız: "Bana bir mesaj ver, ben onu Ack edene (işini bitirene) kadar başka mesaj gönderme" demektir. Bu, en adil dağıtımı sağlar.

- 10 yaparsanız: Tüketiciye tek seferde tampon (buffer) olarak 10 mesaj yığar.

**global** =
- false: Bu limit her yeni tüketici (consumer) için ayrı ayrı uygulanır (Önerilen).
- true: Bu limit tüm kanal (channel) genelinde paylaşılır.

# ExchangeDeclareAsync
RabbitMQ sunucusu üzerinde bir Exchange (Dağıtıcı/Santral) oluşturmak veya var olan bir exchange'in varlığını doğrulamak için kullanılan asenkron metottur. "Idempotent" yapıdadır.

**type:** Exchange'in mesajı nasıl dağıtacağını bu parametre belirler.