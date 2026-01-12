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

## Direct Exchange
RabbitMQ'daki "hedef odaklı" ve seçici çalışan exchange türüdür.

Temel görevi filtrelemedir. Mesajın üzerindeki etikete (Routing Key) bakar ve bu etiketle birebir eşleşen (exact match) kuyruğa mesajı iletir.

**Nasıl Çalışır?**
- **Routing Key (Yönlendirme Anahtarı):** Producer mesajı gönderirken üzerine bir etiket yapıştırır. (Örneğin: kirmizi, hata, resim gibi).
- **Binding Key (Bağlama Anahtarı):** Kuyruklar Exchange'e bağlanırken (bind edilirken) "Ben sadece şu etikete sahip mesajları istiyorum" der.
- **Tam Eşleşme:** Exchange, gelen mesajın Routing Key'i ile kuyruğun Binding Key'ini karşılaştırır. Sadece birebir aynıysa mesajı o kuyruğa atar.

## Topic Exchange
RabbitMQ'daki en esnek ve en yetenekli exchange türüdür.

Temel görevi desen eşleştirmesidir (pattern matching). Direct Exchange gibi sadece tam eşleşmeye bakmaz; kelime grupları ve joker karakterler (wildcards) kullanarak karmaşık yönlendirme kuralları oluşturmanızı sağlar.

**Nasıl Çalışır?**
- Topic Exchange'de Routing Key (Yönlendirme Anahtarı) genellikle noktalarla ayrılmış kelimelerden oluşur. Örnek format: alan.kategori.eylem (örn: araba.bmw.satildi)

**Bu yapıda iki önemli Joker Karakter (Wildcard) kullanılır:**
- (* Yıldız): Tam olarak bir kelimenin yerine geçer.
- (# Kare): Sıfır veya daha fazla (kalan tüm) kelimenin yerine geçer.

### Örnek: Haber Kuyruğu
- avrupa.almanya.spor
- asya.japonya.teknoloji
- avrupa.fransa.ekonomi

kuyrukları olsun elimizde

- (avrupa.\#) -> Avrupa ile ilgili her şeyi gönder (Ülke ve konu fark etmez).

- (\*.almanya.\*) -> Sadece Almanya haberlerini gönder (Kıta ve konu fark etmez).

- (\*.\*.spor) -> Dünyadaki bütün spor haberlerini gönder.
