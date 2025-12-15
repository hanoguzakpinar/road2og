using System;

namespace TaskApp.Console;

public static class TaskHelper
{
    public static async Task<Content> GetContentAsync(string url)
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
}
