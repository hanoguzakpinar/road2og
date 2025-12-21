var myTask = Task.Run(() =>
{
    System.Console.WriteLine("myTask çalıştı.");
});

await myTask;

// Görevin yürütülmesinin bittiğini gösterir. Görev başarılı, hatalı veya iptal edilmiş olsa bile bu değer true döner.
System.Console.WriteLine(myTask.IsCompleted);

//Görevin bir CancellationToken aracılığıyla veya manuel olarak iptal edilerek sonlandığını belirtir.
System.Console.WriteLine(myTask.IsCanceled);

//Görevin yürütülmesi sırasında işlenmemiş bir hata (exception) oluştuğunu gösterir.
System.Console.WriteLine(myTask.IsFaulted);

//Görevin yaşam döngüsündeki tam aşamasını verir. (Running, WaitingForActivation, RanToCompletion, vb.)
//Eğer bir görevin hiçbir hata almadan ve iptal edilmeden, tam olarak başarıyla bittiğinden emin olmak istiyorsanız, Status özelliğini şu şekilde kontrol edebilirsiniz: if (myTask.Status == TaskStatus.RanToCompletion) { ... }
System.Console.WriteLine(myTask.Status);

//(Sadece Task<T> için) Görev bittiğinde döndürdüğü veriyi verir. Görev bitmemişse, bitene kadar mevcut thread'i bloklar.
//myTask.Result

//Görev başlatılırken (StartNew gibi metotlarla) içine konulan opsiyonel durum nesnesidir.
//AsyncState

//Bir görevin hiçbir hata almadan veya iptal edilmeden, tam bir başarıyla sonuçlanıp sonuçlanmadığını tek bir bool değerle bildiren özelliktir.
System.Console.WriteLine(myTask.IsCompletedSuccessfully);