using System.Threading.Tasks;

System.Console.WriteLine("task.WaitAll");
//Task.WaitAll, kendisine verilen bir dizi Task nesnesini girdi olarak alır ve bu görevlerin hepsinin tamamlanmasını bekler.

//En kritik farkı: WaitAll, çağrıldığı iş parçacığını (thread) görevler tamamlanana kadar dondurur (blocklar).

//await Task.WhenAll(...): İş parçacığını serbest bırakır (non-blocking). Uygulamanın yanıt verebilirliği korunur.

//Task.WaitAll(...): İş parçacığını kilitler (blocking). İşlem bitene kadar iş parçacığı hiçbir şey yapamaz.

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

System.Console.WriteLine("waitall'dan önce");

Task.WaitAll(taskList);

//timeout'lu kullanımı
// bool tamamMi = Task.WaitAll(taskList.ToArray(), timeout: TimeSpan.FromSeconds(3));
// System.Console.WriteLine("3 saniyede geldi mi? "+ tamamMi);

System.Console.WriteLine("waitall'dan sonra");

var randomData = taskList.FirstOrDefault();
System.Console.WriteLine($"Url: {randomData.Result.Website} Length: {randomData.Result.Length}");


static async Task<Content> GetContentAsync(string url)
{
    var data = await new HttpClient().GetStringAsync(url);
    Content c = new Content
    {
        Website = url,
        Length = data.Length
    };
    System.Console.WriteLine("Güncel Thread: " + Thread.CurrentThread.ManagedThreadId);
    return c;
}