# Asenkron Programlama
Uygulamanın ana iş parçacığını (thread) engellemeden, potansiyel olarak uzun süren işlemleri (I/O işlemleri, ağ istekleri, dosya erişimi, uzun süren hesaplamalar vb.) eş zamanlı olarak yürütme yeteneğidir.

## Temel Amaç
Temel amaç, uygulamanın kullanıcı arayüzünün (UI) donmasını veya uygulamanın tepkisiz hale gelmesini önlemektir. Özellikle modern grafik arayüzlü (GUI) veya web uygulamalarında, bir sunucudan veri beklerken veya büyük bir dosyayı okurken UI'ın donması kötü bir kullanıcı deneyimi yaratır. Asenkron programlama, bu tür işlemlerin arka planda tamamlanmasını sağlarken, ana iş parçacığının kullanıcı girişlerini işlemeye devam etmesine olanak tanır.

## Senkron ve Asenkron Programlama Karşılaştırması

| Özellik | Senkron (Synchronous) | Asenkron (Asynchronous) |
| :--- | :--- | :--- |
| **Yürütme Biçimi** | Sıralı ve Bloklayıcı | Eş Zamanlı ve Bloklayıcı Olmayan |
| **İş Parçacığı (Thread) Durumu** | İşlem bitene kadar kilitlenir (blocked). | İşlem beklerken serbest bırakılır, diğer görevler için kullanılır. |
| **Tepkisellik** | Uzun işlemlerde uygulama tepkisiz (donmuş) hale gelir. | Uygulama sürekli tepkisel (duyarlı) kalır. |
| **İdeal Kullanım Alanı** | Hızlı, CPU yoğun işlemler. | Uzun süren I/O yoğun işlemler (Ağ, Dosya, Veritabanı). |
| **C# Uygulaması** | Normal metotlar. | `async` ve `await` ile `Task` tabanlı model kullanılır. |

## C# Asenkron Programlamanın Çalışma Mantığı

### 1. Senkron Metot Çağrısı (Bloklayıcı)
Senkron bir metot (void, int, string vb. döndüren normal bir metot) çağrıldığında:

- Program, o metot içindeki kodu satır satır çalıştırmaya başlar.

- Metot içinde uzun süren bir işlemle (örneğin bir web sitesine istek) karşılaşılırsa, tüm iş parçacığı bu işlemin bitmesini bekler.

- Bu süre zarfında iş parçacığı başka hiçbir iş yapamaz. Eğer bu iş parçacığı ana UI iş parçacığı ise, uygulama donar.

### 2. Asenkron Metot Çağrısı (async ve await)
Asenkron bir metot (Task veya Task<T> döndüren async ile işaretlenmiş bir metot) çağrıldığında ise durum farklıdır:

- Görevin Başlatılması: Kod, await ile işaretlenmiş asenkron bir metoda (örneğin client.GetStringAsync(...)) ulaştığında, işlemi başlatır.

- Kontrolün Geri Verilmesi: İşlem başlatılır başlatılmaz, await operatörü kontrolü asenkron metodu çağıran koda geri verir. Bu, asenkron metodun kaldığı yeri (dönüş adresi) bir durum makinesine kaydeder.

- İş Parçacığının Serbest Bırakılması: Bu kritik aşamada, iş parçacığı bekleme moduna geçmek yerine serbest bırakılır ve o anda yapılması gereken diğer işleri (örneğin UI olaylarını işlemek, başka bir kullanıcı isteğine cevap vermek) yapmaya devam eder.

- Görevin Tamamlanması: Arka planda (genellikle işletim sistemi veya donanım tarafından), uzun süren işlem tamamlanır (örneğin web sunucusundan veri gelir).

- Devam Etme: İşlem tamamlandığında, .NET ortamı durum makinesini kullanarak metodu kaldığı yerden (await satırından hemen sonra) çalıştırmaya devam etmek üzere uygun bir iş parçacığı (çoğu zaman orijinal iş parçacığı) üzerinde kuyruğa alır.

- Sonucun Alınması: Metot çalışmaya devam eder ve asenkron işlemden gelen sonucu alır.

## ☕ Senkron vs. Asenkron Programlama: Kafe Örneği

Bu tablo, bloklayıcı (Senkron) ve bloklayıcı olmayan (Asenkron) çalışma mantığını günlük bir senaryo üzerinden karşılaştırmaktadır.

| Özellik | Senkron Kafe (Bloklayıcı) | Asenkron Kafe (Bloklayıcı Olmayan) |
| :--- | :--- | :--- |
| **Rol (Programlama Karşılığı)** | Garson (Ana İş Parçacığı/Thread) | Garson (Ana İş Parçacığı/Thread) |
| **Müşteri (Görev)** | Kahve siparişi (Uzun süren I/O işlemi) | Kahve siparişi (Uzun süren I/O işlemi) |
| **Çalışma Prensibi** | Garson, kahve **hazır olana kadar makinenin başında bekler**. | Garson, kahveyi başlatır ve **beklemek yerine** diğer işlere geçer. |
| **Bekleme Süresi Verimliliği** | Bekleme sırasında (kahve demlenirken) **boş durur**, yeni müşteri alamaz. | Bekleme sırasında masaları temizler veya yeni siparişler alır. |
| **Müşteri Deneyimi (Tepkisellik)**| Yeni müşteriler, eski müşterinin kahvesi bitene kadar **beklemek zorunda kalır**. Kafe donar. | Müşteriler hızlıca sipariş verebilir; kafe her zaman **hizmet vermeye devam eder**. |
| **Sonuç** | Kaynak (Garson/Thread) verimsiz kullanılır. | Kaynak (Garson/Thread) en üst düzeyde verimli kullanılır. |