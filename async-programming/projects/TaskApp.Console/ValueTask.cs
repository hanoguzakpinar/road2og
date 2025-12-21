//ValueTask
//Asenkron programlamada performans optimizasyonu için tasarlanmış, Task sınıfına benzer ancak bellek kullanımı (allocation) açısından çok daha verimli olan bir yapı (struct) türüdür.

//Task bir referans tipidir (class). Her Task oluşturulduğunda bellek yığınında (Heap) bir yer ayrılması gerekir. Eğer asenkron bir metot çok sık çağrılıyor ve çoğu zaman sonucunu anında (senkron olarak) döndürüyorsa (örneğin önbellekten veri okuyorsa), her seferinde Task nesnesi oluşturmak "Garbage Collector" (çöp toplayıcı) üzerinde gereksiz bir yük oluşturur.

//ValueTask bir değer tipidir (struct). Çoğu durumda Heap belleğini kullanmaz, bu da onu yüksek performanslı senaryolarda çok daha hızlı kılar.

// Ne Zaman Kullanılır?
//1- Sonuç Hazırsa: Veri önbellekteyse veya basit bir hesaplama sonucuysa (yani gerçek bir bekleme yapılmayacaksa).
//2- Yüksek Frekans: Metot saniyede binlerce kez çağrılıyorsa.
//3- Performans Kritikse: Bellek yönetimi (Garbage Collection) sürelerini minimize etmek istiyorsanız.

//Task.WhenAll gibi metotlarla doğrudan kullanılamaz; önce .AsTask() metoduna çevrilmesi gerekir.

//Özet: ValueTask, asenkron metotların sonucunun zaten hazır olduğu durumlarda bellek harcamamak için kullanılan, Task'ın yüksek performanslı ve "hafif" (struct) alternatifidir.

using System;
using System.Threading.Tasks;

namespace TaskApp.Console
{
    internal class ValueTask
    {
        public static int cacheData { get; set; } = 150;

        static async Task Main(string[] args)
        {
            var myTask = GetData();
        }

        static ValueTask<int> GetData()
        {
            return new ValueTask<int>(cacheData);
        }
    }
}
