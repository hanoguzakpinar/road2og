using System.Threading.Tasks;

System.Console.WriteLine("task.WaitAny");
//Task.WaitAny, kendisine verilen bir dizi Task nesnesini girdi olarak alır ve bu görevlerden herhangi birinin tamamlanmasını bekler.

//Görevlerden biri tamamlandığında, WaitAny çağrısı hemen geri döner, ancak çağrıyı yapan iş parçacığı o ana kadar bloke edilmiş olur.

//WaitAny metodu, tamamlanan görevin kendisini değil, tamamlanan görev listesindeki indeksini (sıra numarasını) döndürür.

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

System.Console.WriteLine("waitany'dan önce");

var firstTaskIndex = Task.WaitAny(taskList.ToArray());

//timeout'lu kullanımıda mevcuttur.

System.Console.WriteLine("waitany'dan sonra");

var firstData = taskList[firstTaskIndex];
System.Console.WriteLine($"Url: {firstData.Result.Website} Length: {firstData.Result.Length}");


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