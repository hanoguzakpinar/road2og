# Redis
Redis (Remote Dictionary Server), genellikle bir bellek içi (in-memory) anahtar-değer (key-value) veri yapısı deposu olarak kullanılan, açık kaynaklı (BSD lisanslı) bir veri tabanıdır. Aynı zamanda bir önbellekleme (caching) aracı ve mesaj aracısı (message broker) olarak da işlev görebilir.

Redis, verileri diske yazmak yerine RAM'de tuttuğu için çok hızlıdır ve milisaniyelerden daha kısa tepki süreleri sunar. Bu yüksek performans, onu özellikle büyük ölçekli ve yüksek trafikli uygulamalar için ideal kılar.

Redis bir NoSQL veritabanıdır.

## Ana Özellikler ve Kullanım Alanları
1. Yüksek Hız ve Bellek İçi Depolama

- Hız: Redis, verilerin tamamını ana bellekte (RAM) tutarak, geleneksel disk tabanlı veritabanlarının yaşadığı disk I/O (Giriş/Çıkış) gecikmelerini ortadan kaldırır.

- Kalıcılık (Persistence): Temelde bellek içi olmasına rağmen, Redis isteğe bağlı olarak verilerin bir kopyasını diskte tutarak (snapshot alma veya komut loglama yoluyla) sistem yeniden başlatıldığında veri kaybını önleyebilir.

2. Zengin Veri Yapıları

Redis sadece basit dizgileri (string) depolamaz; aynı zamanda gelişmiş ve karmaşık veri yapılarını da destekler, bu da onu çok yönlü yapar:

- Strings (Dizgiler): En temel veri türüdür. Sayı veya metin depolayabilir.

- Lists (Listeler): Ekleme sırasına göre tutulan dizgilerin listeleri (bir kuyruk veya yığın gibi kullanılabilir).

- Sets (Kümeler): Sırasız ve benzersiz dizgiler koleksiyonu.

- Sorted Sets (Sıralı Kümeler): Her üyenin bir puanla ilişkilendirildiği, puana göre sıralanan benzersiz dizgiler (örneğin, skor tabloları için ideal).

- Hashes (Karma Tablolar): Alan-değer çiftlerinin eşlendiği yapılar (nesneleri depolamak için kullanılır).

- Bitmaps ve HyperLogLogs: İleri düzeyde optimizasyonlu, özel amaçlı veri yapıları.

3. Yaygın Kullanım Alanları

Redis'in yüksek performansı ve esnek veri yapıları sayesinde kullanıldığı başlıca alanlar:

- Önbellekleme (Caching): En yaygın kullanım şeklidir. Yavaş disk tabanlı veritabanlarından sıkça okunan verileri hızlı erişim için önbellekte tutar.

- Oturum Yönetimi: Kullanıcı oturumu verilerini (giriş bilgileri, sepet içerikleri vb.) hızlıca saklamak.

- Sıralama ve Kuyruklama (Queues): Listeler veri yapısı sayesinde görev kuyrukları oluşturmak.

- Gerçek Zamanlı Analiz ve Skor Tabloları: Sıralı kümeler (Sorted Sets) kullanarak anlık liderlik tabloları oluşturmak.

- Yayınlama/Abone Olma (Pub/Sub): Basit bir mesajlaşma sistemi olarak işlev görmek.

| Özellik | Redis (Anahtar-Değer Deposu) | İlişkisel Veritabanları (SQL) |
| :--- | :--- | :--- |
| **Depolama** | RAM (**Bellek İçi**) | Disk (Sabit Sürücü) |
| **Hız** | **Çok Hızlı** (Milisaniyenin altı) | Disk I/O nedeniyle daha yavaş |
| **Veri Yapısı** | Anahtar-Değer, Listeler, Kümeler vb. | Sabit Şemalı **Tablolar** |
| **Kullanım Amacı** | Önbellekleme, Oturumlar, Yüksek Hız Gerektiren İşlemler | İşlem Güvenilirliği (ACID), **Karmaşık Sorgular** |