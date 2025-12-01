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

            ViewBag.Zaman2 = _memoryCache.Get<string>("callback");

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

        public IActionResult Expiration()
        {
            if (_memoryCache.TryGetValue("zaman", out string? zamanCache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                };

                //varolan bir cache keyine tekrar bir atama olursa sonuncu geçerli olur, eskisini ezer.
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            return View();
        }

        public IActionResult Priority()
        {
            //Priority = Memory dolduğu zaman cachelerden hangilerinin öncelikle silineceğini belirler.
            //Sıralama = Low, Normal, High, NeverRemove

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                //SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };

            //Cache elemanı silindiği anda otomatik tetiklenen bir event
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"Key:{key} - Value:{value} - Reason:{reason} - State:{state}");
            });

            //varolan bir cache keyine tekrar bir atama olursa sonuncu geçerli olur, eskisini ezer.
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            return View();
        }
    }
}