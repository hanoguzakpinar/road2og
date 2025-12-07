using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace MyApp.Namespace
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase _db;

        public StringTypeController(RedisService rediService)
        {
            _redis = rediService;
            _db = _redis.GetDb(0);
        }

        public ActionResult Index()
        {
            _db.StringSet("name", "oguzhan akpinar");
            _db.StringSet("ziyaretci", 100);

            return Content("index");
        }

        public IActionResult Show()
        {
            var value = _db.StringGet("name");

            _db.StringIncrement("ziyaretci", 10);

            var count = _db.StringDecrementAsync("ziyaretci", 1).Result;
            //sonucu dönmeden asenkron işlemi yap demektir.
            _db.StringDecrementAsync("ziyaretci", 10).Wait();

            if (value.HasValue)
            {
                return Content(value.ToString());
            }
            return Content(count.ToString());
        }
    }
}
