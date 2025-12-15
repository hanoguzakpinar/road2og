using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TaskApp.Console;

namespace TaskApp.ConsoleApp
{
    internal class Delay
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("task.Delay");
            //Task.Delay, adından da anlaşılacağı gibi, yürütmeyi geciktirmek veya duraklatmak için tasarlanmıştır.
            //Bir iş parçacığını bloklamadan (non-blocking) belirli bir süre beklemek için kullanılır. Thread.Sleep gibi bloklamaz.

            System.Console.WriteLine("Main Thread: " + Thread.CurrentThread.ManagedThreadId);

            List<string> urls = new List<string>
            {
                "https://www.github.com/",
                "https://www.google.com/",
                "https://www.microsoft.com/"
            };

            List<Task<Content>> taskList = new List<Task<Content>>();

            urls.ForEach(x =>
            {
                taskList.Add(GetContentAsync(x));
            });

            var contents = await Task.WhenAll(taskList.ToArray());

            contents.ToList().ForEach(c =>
            {
                System.Console.WriteLine($"Url: {c.Website} Length: {c.Length}");
            });
        }

        private static async Task<Content> GetContentAsync(string url)
        {
            var data = await new HttpClient().GetStringAsync(url);

            await Task.Delay(5000);

            Content c = new Content
            {
                Website = url,
                Length = data.Length
            };
            System.Console.WriteLine("Güncel Thread: " + Thread.CurrentThread.ManagedThreadId);
            return c;
        }
    }
}
