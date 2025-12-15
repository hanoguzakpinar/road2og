using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TaskApp.Console;

namespace TaskApp.ConsoleApp
{
    class WhenAll
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("task.WhenAll");
            //Task.WhenAll, bir dizi (koleksiyon) Task nesnesini girdi olarak alır ve bu görevlerin hepsinin tamamlanmasını bekleyen tek bir görev (Task) döndürür.

            //Basitçe ifade etmek gerekirse: "Bu listedeki tüm işler bitmeden bir sonraki adıma geçme."


            System.Console.WriteLine("Main Thread: " + Thread.CurrentThread.ManagedThreadId);

            List<string> urls = new List<string>
            {
                "https://www.google.com/",
                "https://www.github.com/",
                "https://www.microsoft.com/"
            };

            List<Task<Content>> taskList = new List<Task<Content>>();

            urls.ForEach(x =>
            {
                taskList.Add(TaskHelper.GetContentAsync(x));
            });

            var contents = await Task.WhenAll(taskList);

            contents.ToList().ForEach(c =>
            {
                System.Console.WriteLine($"Url: {c.Website} Length: {c.Length}");
            });
        }
    }
}