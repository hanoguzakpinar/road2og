using System;
using System.Threading.Tasks;

namespace TaskApp.Console;

public class TaskResult
{
    public TaskResult()
    {
        System.Console.WriteLine("task.Result");
        System.Console.WriteLine(GetData());
    }

    //task.Result özelliği, asenkron bir görevin (Task) ürettiği nihai sonuca erişmek için kullanılır. Eğer metodunuz Task<T> dönüyorsa, buradaki T tipindeki gerçek veriyi Result ile çekersiniz.

    //Senkron (Blocking): Görevi beklerken mevcut iş parçacığını kilitler.

    //await modern asenkron uygulamalarda yanıt verebilirliği korumak için, .Result ise yalnızca asenkron desteği olmayan eski/zorunlu senkron yapılarda sonucu zorla almak için kullanılır.

    //Metot imzası void ise ve asenkron yapıya çevrilemiyorsa "son çare" olarak.

    static string GetData()
    {
        var task = new HttpClient().GetStringAsync("https://google.com");

        var data = task.Result;

        return data;
        // bu kullanımda thread bloklar.
    }

    static async Task<string> GetData2()
    {
        var task = new HttpClient().GetStringAsync("https://google.com");

        await task;

        return task.Result;
        // bu kullanımda thread bloklamaz.
    }

    static async Task<string> GetData3()
    {
        var task = new HttpClient().GetStringAsync("https://google.com").ContinueWith((data) =>
        {
            //GetStringAsync metodundan sonuç geldi, o metot bittiği için bu kullanımda thread bloklamaz.
            return data.Result;
        });
        return string.Empty;
    }
}
