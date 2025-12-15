using System;
using System.Threading;
using System.Threading.Tasks;

 //Task.Run, kendisine verilen bir işlevi veya kodu, .NET İş Parçacığı Havuzundaki (Thread Pool) bir iş parçacığı üzerinde asenkron olarak çalıştırmak için kullanılır.

 //Basitçe ifade etmek gerekirse: "Bu kodu ana akıştan ayır ve arka planda, boş bir iş parçacığında çalıştır."

 //Task.Run'ın birincil amacı, CPU Yoğunluklu (CPU-bound) görevleri (uzun süren hesaplamalar, döngüler, veri işleme) ana iş parçacığından (özellikle kullanıcı arayüzü veya sunucu istek iş parçacığı) ayırarak uygulamanın yanıt verebilirliğini korumaktır.

 //Task.Run, genellikle eşzamanlı (synchronous) çalışan kod bloklarını alıp onları asenkron bir bağlamda çalıştırmak için bir köprü görevi görür.

 /* Task.Run'ın İki Yaygın Kullanım Senaryosu
  1. Senkron Metotları Asenkron Yapmak: Mevcut bir kütüphanedeki veya eski koddaki asenkron alternatifi olmayan senkron bir metodu (örneğin bir dosya okuma metodu) asenkron bir bağlamda çalıştırmak için.
  2. Aşırı CPU Yükünü Ayırmak: Bir web sunucusu (API) veya masaüstü uygulamasının Ana İş Parçacığından yoğun hesaplama gerektiren görevleri ayırmak için kullanılır.
 */

namespace TaskApp.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            System.Console.WriteLine("task.Run");
            await HesaplaAsync();
        }

        static async Task HesaplaAsync()
        {
            // 1. Durum güncellemesi
            System.Console.WriteLine("Hesaplama başladı. Lütfen bekleyin...");

            // 2. Task.Run: Uzun süren senkron metodu arka plandaki bir Thread Pool
            // iş parçacığında çalıştırırız. UI Thread serbest kalır.
            long sonuc = await Task.Run(() => UzunSurenHesaplama(5000));
            
            // 3. await bittiğinde tekrar Ana Thread'e döneriz (UI uygulamalarında).
            System.Console.WriteLine($"Hesaplama bitti. Sonuç: {sonuc}");
        }

        // 5 saniye süren, işlemci yoğunluklu (CPU-bound) senkron metot
        static long UzunSurenHesaplama(int sureMs)
        {
            Thread.Sleep(sureMs); // Gerçekte burada karmaşık matematiksel işlemler olur.
            return 123456789;
        }
    }
}