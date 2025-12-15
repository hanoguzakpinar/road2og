using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TaskApp.Console;

class WaitAny
{
    static void Main(string[] args)
    {
        Console.WriteLine("task.WaitAny");
        //Task.WaitAny, kendisine verilen bir dizi Task nesnesini girdi olarak alır ve bu görevlerden herhangi birinin tamamlanmasını bekler.

        //Görevlerden biri tamamlandığında, WaitAny çağrısı hemen geri döner, ancak çağrıyı yapan iş parçacığı o ana kadar bloke edilmiş olur.

        //WaitAny metodu, tamamlanan görevin kendisini değil, tamamlanan görev listesindeki indeksini (sıra numarasını) döndürür.

        Console.WriteLine("Main Thread: " + Thread.CurrentThread.ManagedThreadId);

        List<string> urls = new List<string>
        {
            "https://www.github.com/",
            "https://www.google.com/",
            "https://www.microsoft.com/"
        };

        List<Task<Content>> taskList = new List<Task<Content>>();

        urls.ForEach(x =>
        {
            taskList.Add(TaskHelper.GetContentAsync(x));
        });

        Console.WriteLine("waitany'dan önce");

        var firstTaskIndex = Task.WaitAny(taskList.ToArray());

        //timeout'lu kullanımıda mevcuttur.

        Console.WriteLine("waitany'dan sonra");

        var firstData = taskList[firstTaskIndex];
        Console.WriteLine($"Url: {firstData.Result.Website} Length: {firstData.Result.Length}");
    }
}