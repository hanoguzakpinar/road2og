//Task.StartNew metodu, C#'taki GörevParalel Kütüphanesi (TPL) içinde görevoluşturup hemen başlatan eski bir yöntemdir.Günümüzde, Task.Run yaygın olarak tercihedildiği için, Task.StartNew metodugenellikle bir antipattern (kaçınılmasıgereken bir kullanım biçimi) olarak görülür.

//Task.StartNew ile bir UI iş parçacığındaişlem başlattığınızda, bu işlem bazen arkaplan yerine yine UI iş parçacığındaçalışmaya başlayabilir. Bu da uygulamanızındonmasına yol açar. Task.Run ise bu tür birkilitlenmeyi garantili olarak önler.

//Task.Factory.StartNew metodu, statik bir metot olmaktan ziyade, TaskScheduler yapısı aracılığıyla bir görev oluşturur ve hemen yürütülmek üzere sıraya alır.

public class StartNew
{
    public async Task StartNewMain()
    {
        System.Console.WriteLine("task.Factory.StartNew");

        var myTask = Task.Factory.StartNew((obj) =>
        {
            System.Console.WriteLine("myTask çalıştı.");
            var status = obj as Status;
            status.ThreadId = Thread.CurrentThread.ManagedThreadId;
        }, new Status { Date = DateTime.Now });

        await myTask;

        Status s = myTask.AsyncState as Status;
        System.Console.WriteLine("date: " + s.Date);
        System.Console.WriteLine("thread: " + s.ThreadId);
    }
}

public class Status
{
    public int ThreadId { get; set; }
    public DateTime Date { get; set; }
}