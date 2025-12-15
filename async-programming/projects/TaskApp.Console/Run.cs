 System.Console.WriteLine("task.Run");
 //Task.Run, kendisine verilen bir işlevi veya kodu, .NET İş Parçacığı Havuzundaki (Thread Pool) bir iş parçacığı üzerinde asenkron olarak çalıştırmak için kullanılır.

 //Basitçe ifade etmek gerekirse: "Bu kodu ana akıştan ayır ve arka planda, boş bir iş parçacığında çalıştır."

 //Task.Run'ın birincil amacı, CPU Yoğunluklu (CPU-bound) görevleri (uzun süren hesaplamalar, döngüler, veri işleme) ana iş parçacığından (özellikle kullanıcı arayüzü veya sunucu istek iş parçacığı) ayırarak uygulamanın yanıt verebilirliğini korumaktır.

 