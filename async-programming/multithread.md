# Multithread Programlama
Bir programın (process) içinde birden fazla iş parçacığının (thread) aynı anda çalıştırılmasını ifade eder.

- **Process (Süreç):** Çalışan bir programın bağımsız bir örneğidir. Kendi bellek alanına, kaynaklarına ve en az bir iş parçacığına sahiptir.

- **Thread (İş Parçacığı):** Bir process içindeki yürütmenin en küçük birimidir. İşlemci tarafından ayrı ayrı zamanlanabilen ve çalıştırılabilen komut akışıdır. Aynı process'e ait iş parçacıkları, process'in bellek alanını ve kaynaklarını (dosyalar, vb.) paylaşır.

**Temel Fikir:** Ağır veya uzun süren görevleri (dosya okuma/yazma, ağ bağlantısı kurma, karmaşık hesaplamalar) ana akıştan (UI/kullanıcı arayüzü) ayırarak uygulamanın donmadan çalışmaya devam etmesini sağlamaktır.

## Neden Multithreading Kullanılır?
- **Yanıt Verebilirlik (Responsiveness):** Uygulamanın ana iş parçacığı (genellikle UI iş parçacığı) meşgul edilmediği için, kullanıcı arayüzü donmaz ve kullanıcı etkileşimlerine (düğmeye tıklama, kaydırma) anında yanıt vermeye devam eder.

- **Performans ve Verimlilik:** Çok çekirdekli işlemcilerin gücünden yararlanarak, birden fazla iş parçacığı gerçekten eşzamanlı (aynı anda) çalışabilir, bu da toplam yürütme süresini kısaltır.

- **Kaynak Kullanımı:** G/Ç (Input/Output) beklerken (örneğin, veritabanından veri okunurken), işlemci boş durmak yerine başka bir iş parçacığındaki işi yapabilir.

## Örnek 1
**Senaryo: Bir Masaüstü Uygulamasında (WinForms/WPF) Büyük Bir Raporun İndirilmesi**

Bir uygulamanız var ve kullanıcı bir düğmeye basarak birkaç saniye sürecek karmaşık bir veritabanı sorgusu çalıştırmak veya internetten büyük bir dosya indirmek istiyor.

❌ Multithreading Olmasaydı (Senkron Çalışma)
- Kullanıcı düğmeye basar.

- Ana İş Parçacığı (UI Thread) raporu indirme veya hesaplama işine başlar.

- İşlem bitene kadar (örneğin 5 saniye) UI Thread kilitlenir.

- Kullanıcı arayüzü donar, hareket etmez, düğmeye tekrar basamaz, pencereyi sürükleyemez.

- 5 saniye sonra işlem biter ve UI tekrar yanıt vermeye başlar.

<br>

✅ Multithreading ile
| İş Parçacığı | Ne yapar? |
|---|---|
| Ana İş Parçacığı (UI Thread) | Kullanıcının düğmeye tıklamasını işler ve indirme görevini başlatır. |
| Arka Plan (Thread Pool) İş Parçacığı | Gerçek ağ/disk (I/O) işlemini yürütür. |
| Ana İş Parçacığı (UI Thread) | İndirme işlemi beklenirken serbest kalır. UI yanıt vermeye devam eder. |

## Örnek 2
**Senaryo: Bir Web API'sinde Gelen Verilerin Aynı Anda İşlenmesi**

Bir e-ticaret uygulamasının arka ucunda, gelen bir siparişteki her bir ürün için ayrı ayrı stok kontrolü ve fiyat doğrulaması yapılması gerekiyor.

**İşlem:**
- Siparişin içinde 10 farklı ürün var.
- Her bir ürünün kontrolü 200 ms sürsün.
- Toplam süre (senkron): $10 \times 200 \text{ ms} = 2000 \text{ ms}$ (2 saniye).

<br>

✅ Multithreading ile
| İş Parçacığı | Ne yapar? |
|---|---|
| Thread 1 | Ürün 1 ve 2'nin kontrolünü yapar. |
| Thread 2 | Ürün 3, 4 ve 5'in kontrolünü yapar. |
| Thread 3 | Ürün 6, 7 ve 8'in kontrolünü yapar. |
| Thread 4 | Ürün 9 ve 10'un kontrolünü yapar. |

**Sonuç:** Toplam süre, işlemci çekirdeği sayısına ve işin yapısına bağlı olarak 2 saniyeden çok daha kısa (belki 500 ms - 700 ms) sürer. Bu, yüksek trafikli sunucu uygulamalarında verimliliği katlar.