using System.Threading.Tasks;

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
    taskList.Add(GetContentAsync(x));
});

var contents = await Task.WhenAll(taskList);

contents.ToList().ForEach(c =>
{
    System.Console.WriteLine($"Url: {c.Website} Length: {c.Length}");
});

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

class Content
{
    public string Website { get; set; } = string.Empty;
    public int Length { get; set; }
}