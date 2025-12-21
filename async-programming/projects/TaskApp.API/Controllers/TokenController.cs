using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetContentAsync()
        {
            //Bu endpointe istek atıp, kullanıcı tarayıcı kapattı, cancellation token olmadığı için, tarayıcı kapansa bile arka planda bu istek işlenmeye devam eder.
            _logger.LogInformation("İstek başladı.");

            Thread.Sleep(5000);

            var myTask = new HttpClient().GetStringAsync("https://www.google.com");

            var data = await myTask;

            _logger.LogInformation("İstek bitti.");

            return Ok(data);
        }
        [HttpGet("cancellationToken")]
        public async Task<IActionResult> GetContentAsync(CancellationToken ct)
        {
            //Bu endpointe istek atıp, kullanıcı tarayıcı kapattı, cancellation token olduğu için task cancel edilecek.
            try
            {
                _logger.LogInformation("İstek başladı.");

                await Task.Delay(5000, ct);

                var myTask = new HttpClient().GetStringAsync("https://www.google.com");

                var data = await myTask;

                _logger.LogInformation("İstek bitti.");

                //manuel kendimizde bu metot ile hata fırlatabiliriz. bu metot cancellation token kullandı mı diye bakar, kullanıldıysa hata fırlatır.
                //ct.ThrowIfCancellationRequested();

                return Ok(data);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogInformation($"İstek iptal edildi. {ex.Message}");
                return BadRequest();
            }
        }
    }
}
//Cancellation Token
//CancellationToken, C# ve .NET dünyasında çalışan bir asenkron işlemin veya iş parçacığının (thread) dışarıdan iptal edilmesini sağlamak için kullanılan standart bir mekanizmadır.

//Bir kullanıcı butona bastığında indirme işleminin durması, bir web isteğinin zaman aşımına uğraması veya uygulamanın kapanması gibi durumlarda çalışan görevleri güvenli bir şekilde sonlandırmak için kullanılır.