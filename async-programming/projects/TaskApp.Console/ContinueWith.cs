using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TaskApp.ConsoleApp
{
    class ContinueWith
    {
        static async Task Main()
        {
            System.Console.WriteLine("task.ContinueWith");
            //ContinueWith metodu, bir Task tamamlandıktan hemen sonra otomatik olarak çalıştırılması gereken başka bir Task tanımlamanızı sağlar.
            //Basitçe ifade etmek gerekirse: "Bu işi bitirince, hemen arkasından şunu yap." demenin bir yoludur.

            // var task = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith((data) =>
            // {//data Task<string> türünde olduğu için sonuç data.Result propertysinden alınır
            //     Console.WriteLine($"uzunluk: {data.Result.Length}");
            // });

            var task = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith(print);

            System.Console.WriteLine("arada yapılacak işler");

            await task;
        }

        static void print(Task<string> task)
        {
            //task Task<string> türünde olduğu için sonuç data.Result propertysinden alınır
            System.Console.WriteLine($"uzunluk: {task.Result.Length}");
        }
    }
}