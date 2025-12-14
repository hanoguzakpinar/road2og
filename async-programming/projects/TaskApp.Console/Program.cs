using System.Threading.Tasks;

System.Console.WriteLine("task.WhenAny");
//Task.WhenAny, kendisine verilen görev (Task) listesinden herhangi birinin tamamlanmasını bekleyen tek bir görev (Task) döndürür.

//Basitçe ifade etmek gerekirse: "Bu listedeki işlerden hangisi önce biterse, onunla ilgilen ve hemen devam et."

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
    taskList.Add(GetContentAsync(x));
});

var firstData = await Task.WhenAny(taskList);
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