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

        //aşağıdaki metotta async ve await keywordleri yok.
        // await olmadığında:
        // “Bu iş başladı, ben handle’ını (Task) sana veriyorum; bittiğinde içinde sonuç olacak” diyorsun.
        // Metot hemen döner; sonucu alma işi çağıran tarafa (caller) kalır: caller await eder, Result okur vs.
        // Exception olursa, exception Task’ın içine yazılır; caller onu await edince (veya Result deyince) ortaya çıkar.

        // await olsaydı:
        // Metot içinde “iş bitsin, sonucunu alayım, sonra devam edeyim” diyorsun.
        // Beklemek = thread’i bloklamak değil. Metot o noktada kontrolü runtime’a bırakıyor; iş bitince devam ediyor.
        // Sonucu metot içinde kullanabildiğin için Ok(data) gibi bir cevap üretebiliyorsun.
        private Task<string> GetContent()
        {
            return new HttpClient().GetStringAsync("http://google.com");
        }
        // özet
        // await kullanırsan metot iş bitene kadar asenkron biçimde durup sonucu burada işleyerek döner; await kullanmazsan metot beklemeden sadece devam eden işin Task’ını döner ve sonucu/hatasını çağıran taraf iş bitince alır.

        [HttpGet("content")]
        public async Task<IActionResult> GetContentAsync2()
        {
            //GetContent() → Task<string> döndürür (henüz sonuç değil, “sonuç gelecek” demek)
            // await ile Task tamamlanana kadar asenkron bekleyip content değişkenine gerçek string’i alırsın.
            var content = await GetContent();
            return Ok(content);
        }
    }
}
