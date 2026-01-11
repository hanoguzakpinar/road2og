# Exchange
RabbitMQ'nun mesaj yönlendiricisidir.

Producer'dan (Üretici) mesajı teslim alır ve onu hangi Kuyruğa (Queue) atması gerektiğine karar verir.

## Fanout Exchange
RabbitMQ’daki en basit ve en hızlı çalışan exchange türüdür. Temel görevi yayın (broadcast) yapmaktır.

Producer'dan gelen bir mesajı, üzerinde herhangi bir filtreleme yapmadan (Routing Key'e bakmadan), kendisine bağlı (bind edilmiş) bütün kuyruklara kopyalar.

**Nasıl Çalışır?**
- **Routing Key Yok Sayılır:** Producer mesajı gönderirken bir "adres/etiket" (routing key) belirtse bile, Fanout Exchange bunu görmezden gelir.
- **Çoğaltma (Duplication):** Exchange, kendisine o an kaç tane kuyruk bağlıysa, mesajı hepsine tek tek kopyalar.
- **Hız:** Herhangi bir string eşleştirmesi veya kontrolü yapmadığı için diğer exchange türlerine göre (Direct veya Topic) daha performanslıdır.