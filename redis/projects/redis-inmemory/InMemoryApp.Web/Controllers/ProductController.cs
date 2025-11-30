using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //1.yol cache var mı kontrolü
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            //2.yol cache var mı kontrolü
            if (_memoryCache.TryGetValue("zaman", out string? zamanCache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            return View();
        }

        public IActionResult Show()
        {
            ViewBag.Zaman = _memoryCache.Get<string>("zaman");

            return View();
        }

        public IActionResult Remove()
        {
            //cacheden silme
            _memoryCache.Remove("zaman");

            return View();
        }

        public IActionResult GetOrCreate()
        {
            //cache'den okumaya çalışır, yoksa oluşturur
            _memoryCache.GetOrCreate<string>("zaman", c =>
            {
                //c.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
                return DateTime.Now.ToString();
            });

            return View();
        }
    }
}