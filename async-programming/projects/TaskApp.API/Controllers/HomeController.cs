using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetContentAsyc()
        {
            var myTask = new HttpClient().GetStringAsync("http://google.com");

            //yukarıdaki metot işlemini bitirene kadar burada başka işlerde yapılabilir. loglama vs.

            //sonuç gelene kadar bekle. sonucu aldıktan sonra devam et. (thread bloklamaz)
            var data = await myTask;

            return Ok(data);
        }
    }
}
