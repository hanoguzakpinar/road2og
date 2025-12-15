 System.Console.WriteLine("task.Run");
 //Task.Run, kendisine verilen bir işlevi veya kodu, .NET İş Parçacığı Havuzundaki (Thread Pool) bir iş parçacığı üzerinde asenkron olarak çalıştırmak için kullanılır.

 //Basitçe ifade etmek gerekirse: "Bu kodu ana akıştan ayır ve arka planda, boş bir iş parçacığında çalıştır."

 //Task.Run'ın birincil amacı, CPU Yoğunluklu (CPU-bound) görevleri (uzun süren hesaplamalar, döngüler, veri işleme) ana iş parçacığından (özellikle kullanıcı arayüzü veya sunucu istek iş parçacığı) ayırarak uygulamanın yanıt verebilirliğini korumaktır.

 //Task.Run, genellikle eşzamanlı (synchronous) çalışan kod bloklarını alıp onları asenkron bir bağlamda çalıştırmak için bir köprü görevi görür.

 /* Task.Run'ın İki Yaygın Kullanım Senaryosu
  1. Senkron Metotları Asenkron Yapmak: Mevcut bir kütüphanedeki veya eski koddaki asenkron alternatifi olmayan senkron bir metodu (örneğin bir dosya okuma metodu) asenkron bir bağlamda çalıştırmak için.
  2. Aşırı CPU Yükünü Ayırmak: Bir web sunucusu (API) veya masaüstü uygulamasının Ana İş Parçacığından yoğun hesaplama gerektiren görevleri ayırmak için kullanılır.
 */