using System;

namespace TaskApp.Console;

public class FromResult
{
    public FromResult()
    {
        System.Console.WriteLine("task.FromResult");
    }
    //Task.FromResult, halihazırda elinizde bulunan bir değeri (sonucu), sanki asenkron bir operasyondan dönmüş gibi bir Task<T> içine paketlemek için kullanılır.

    //Ortada bekleyecek (await edilecek) gerçek bir asenkron iş yoktur, ancak metodun imzası bir Task döndürmek zorundadır. İşte bu durumlarda Task.FromResult imdadınıza yetişir.

    //Neden Kullanılır?
    //1- Interface (Arayüz) Uyumluluğu: Bir arayüzde tanımlanan metot Task<int> döndürüyorsa, ancak sizin yazdığınız sınıftaki (implementasyon) bu metot sonucu anında (senkron olarak) hesaplayabiliyorsa kullanılır.
    // 2- Caching (Önbellekleme): Veriyi bir kez asenkron çekip belleğe attıktan sonra, sonraki çağrılarda veriyi ağa gitmeden anında döndürmek istediğinizde kullanılır.

    //Özet
    //Ne yapar? Senkron bir değeri, asenkron bir Task nesnesine dönüştürür.
    //Performans: Çok hızlıdır, çünkü yeni bir iş parçacığı (thread) başlatmaz.
    //Ne zaman kullanılır? Asenkron bir metot imzasına sahip olup, sonucu anında döndürmeniz gereken durumlarda.
}
