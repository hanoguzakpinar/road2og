# Caching
Daha önce elde edilmiş verileri veya hesaplanmış sonuçları bellekte (RAM veya başka hızlı depolama alanları) geçici olarak saklama mekanizmasıdır. Bu, uygulamanın aynı veriye veya sonucu tekrar tekrar ihtiyaç duyduğunda ağır iş yükünü (örneğin bir veritabanı sorgusu, harici API çağrısı veya karmaşık bir hesaplama) tekrar yapmasını engeller.

Temel amaç, performansı artırmak ve sunucu kaynaklarının kullanımını azaltmaktır.

## Caching Türleri
- Bellek İçi Önbellekleme (In-Memory Caching)
- Dağıtılmış Önbellekleme (Distributed Caching)

### Bellek İçi Önbellekleme (In-Memory Caching)
**Tanım:** Verilerin doğrudan web sunucusunun (veya uygulamanın) kendi belleğinde (RAM) saklanmasıdır.

**Kullanım Alanı:** Tek bir sunucu üzerinde çalışan uygulamalar veya veri tutarlılığının anlık olarak kritik olmadığı senaryolar için idealdir.

**Sınırlama:** Uygulama veya sunucu yeniden başlatıldığında önbellek kaybolur. Birden fazla sunucuda (web farm) kullanıldığında, her sunucunun kendi önbelleği olacağından veri tutarlılığı sorunları yaşanabilir.

### Dağıtılmış Önbellekleme (Distributed Caching)
**Tanım:** Verilerin uygulamanın kendi belleği yerine harici bir hizmette (örneğin Redis veya SQL Server) saklanmasıdır.

**Kullanım Alanı:** Birden fazla sunucunun kullanıldığı (web farm) ortamlarda, tüm sunucuların aynı önbellek verisine erişmesini sağlamak için zorunludur.

**Avantaj:** Sunucular yeniden başlatılsa bile önbellek verileri kalır. Tüm uygulama örnekleri arasında tutarlılık sağlar.

## On-Demand & PrePopulation Caching
### On-Demand
**Tanım:** Veri, yalnızca kullanıcı tarafından talep edildiği an önbelleğe yazılır.

Aynı zamanda Lazy Loading (Tembel Yükleme) veya Cache-Aside Pattern olarak da bilinen en yaygın önbellekleme stratejisidir.

**Çalışma Şekli:**
- Uygulama, bir veri isteği alır.
- Önce önbelleği kontrol eder (Cache Hit mi, Miss mi?).
- Eğer veri önbellekte yoksa (Cache Miss), veriyi asıl veri kaynağından (Veritabanı, Harici API vb.) alır.
- Veriyi kullanıcıya döndürmeden önce, aynı anda önbelleğe yazar (Cache Miss durumuyla beraber asıl veri kaynağından verinin elde edilip önbellek alanına yazılmasına dayanır).
- Bir sonraki aynı istek, önbellekten hızlıca karşılanır.

**Avantajları**
- Kaynak Verimliliği: Yalnızca gerçekten kullanılan veriler önbellekte tutulur, bu sayede önbellek alanı boşa harcanmaz.
- Küçük Önbellek Alanı: Çok büyük veri setlerinde bile önbellek boyutu, aktif kullanılan verilerle sınırlı kalır.

**Dezavantajları**
- İlk Erişim Yavaşlığı: Veriye ilk kez erişen kullanıcı, verinin kaynağına gidilmesini ve önbelleğe yazılmasını beklemek zorundadır. Bu, ilk istek için gecikmeye (latency) neden olur.

### Pre-Population Caching
**Tanım:** Veri, daha kullanıcıdan talep gelmeden, genellikle uygulama başlarken (startup) veya önceden belirlenmiş aralıklarla önbelleğe yüklenir.

**Çalışma Şekli:**
- Uygulama başlar veya belirlenen bir zamanlayıcı tetiklenir.
- Uygulama, sıkça kullanılacağı tahmin edilen verileri (örneğin şehir listeleri, ayarlar, popüler ürünler) asıl veri kaynağından (veritabanı) çeker.
- Bu verileri önbelleğe yazar.
- Kullanıcı bir istek gönderdiğinde, veri zaten önbellekte hazırdır ve anında (Cache Hit) cevaplanır.

**Avantajları**
- Sıfır Gecikme: Veriye ilk kez erişim dahil, tüm erişimler için en hızlı yanıt süresini (düşük latency) sağlar. Kullanıcı her zaman hızlı bir deneyim yaşar.
- Veritabanı Yükünü Azaltma: Uygulama başlangıcında tek seferlik bir yük oluşturur ve ardından veritabanı üzerindeki yük büyük ölçüde azalır.

**Dezavantajları**
- Kaynak Tüketimi: Kullanılmayacak olsa bile tüm veriler bellekte yer kaplar.
- Uygulama Başlangıç Yavaşlığı: Uygulamanın veya sunucunun açılması, veriler yüklenirken yavaşlayabilir.
- Bayat Veri Riski: Veriler önceden yüklendiği için, kaynakta bir güncelleme olursa önbellekteki verinin güncel olmayan (stale) olma riski On-Demand'e göre daha yüksektir.

| Özellik | On-Demand Caching (İhtiyaç Anında) | Pre-Population Caching (Önceden Doldurmalı) |
| :--- | :--- | :--- |
| **Önbelleğe Alma Zamanı** | Kullanıcı ilk kez talep ettiğinde (Cache Miss anında) | Uygulama başlangıcında veya periyodik olarak |
| **İlk İstek Performansı** | Yavaş (Veri kaynağına gidilir) | Çok Hızlı (Veri zaten hazır) |
| **Önbellek Alanı** | Verimli kullanılır (Sadece kullanılanlar tutulur) | Daha fazla kaynak tüketimi (Tüm veri tutulur) |
| **Bayat Veri Riski** | Daha Düşük | Daha Yüksek |
| **İdeal Kullanım Alanı** | Çok büyük veri setleri, nadiren erişilen veriler | Statik veriler, küçük/orta boyutta, sık erişilen veriler |

## Cache Ömürleri
### Absolute Time
**Tanım:** Bir önbellek öğesi, önbelleğe eklendiği andan itibaren belirlenen sabit bir süre sonunda (örneğin 5 dakika) otomatik olarak geçersiz kılınır ve kaldırılır.

**Çalışma Şekli:** Öğeye bu süre zarfında kaç kez erişildiği önemli değildir. Süre dolduğunda, önbellek öğesi anında silinir.

**Kullanım Alanı:** Verilerin belirli bir süre sonunda kesinlikle güncellenmesi gereken veya verinin ne kadar sık kullanıldığına bakılmaksızın belirli aralıklarla yenilenmesi gereken senaryolar için idealdir. (Örneğin, döviz kurları, hava durumu verileri).

**Örnek:** Bir ürünü önbelleğe aldınız ve Mutlak Süre Sonu 10 dakika olarak ayarlandı. 9 dakika 59 saniye boyunca binlerce kişi bu ürüne erişse bile, 10. dakikada bu öğe önbellekten atılır.

**Dezavantaj:** Sürekli kullanılan popüler bir veri bile süresi dolduğunda önbellekten atılır ve bir sonraki istekte yeniden yüklenir, bu da geçici bir yavaşlamaya neden olabilir.

### Sliding Time
**Tanım:** Bir önbellek öğesi, belirlenen bir süre boyunca kendisine erişilmediği takdirde geçersiz kılınır ve kaldırılır.

**Çalışma Şekli:** Her erişim, öğenin ömrünü sıfırlar (sanki yeni eklenmiş gibi). Eğer erişim aralığı belirlenen süreden (örneğin 2 dakika) daha uzun olursa, öğe önbellekten atılır.

**Örnek:** Bir kullanıcının oturum bilgisi önbelleğe alındı ve Kayan Süre Sonu 20 dakika olarak ayarlandı. Kullanıcı sisteme her 15 dakikada bir erişirse, bilgi önbellekte kalmaya devam eder. Eğer 21 dakika boyunca hiçbir işlem yapmazsa, önbellek öğesi silinir.

**Kullanım Alanı:** Sık kullanılan (popüler) verilerin önbellekte tutulmasını, nadiren kullanılan verilerin ise temizlenmesini sağlamak için kullanılır. Özellikle kullanıcı oturumları, sepet içerikleri veya popüler ürün listeleri gibi durumlarda faydalıdır.

**Dezavantaj:** Veri, sürekli erişildiği takdirde süresiz olarak önbellekte kalabilir. Bu nedenle, Kayan Süre Sonu genellikle bir Mutlak Süre Sonu (örneğin 24 saat) ile birleştirilerek, verinin sonsuza kadar önbellekte kalması engellenir.