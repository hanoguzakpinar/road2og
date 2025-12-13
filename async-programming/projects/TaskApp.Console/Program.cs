System.Console.WriteLine("task.ContinueWith");

// var task = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith((data) =>
// {//data Task<string> türünde olduğu için sonuç data.Result propertysinden alınır
//     Console.WriteLine($"uzunluk: {data.Result.Length}");
// });

var task = new HttpClient().GetStringAsync("https://www.google.com").ContinueWith(print);

System.Console.WriteLine("arada yapılacak işler");

await task;

static void print(Task<string> task)
{
    //task Task<string> türünde olduğu için sonuç data.Result propertysinden alınır
    Console.WriteLine($"uzunluk: {task.Result.Length}");
}