using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace MyApp.Namespace
{
    public class ListController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase _db;
        private string listKey = "names";
        public ListController(RedisService rediService)
        {
            _redis = rediService;
            _db = _redis.GetDb(1);
        }
        public ActionResult Index()
        {
            var names = new List<string>();
            if (_db.KeyExists(listKey))
            {
                _db.ListRange(listKey).ToList().ForEach(f =>
                {
                    names.Add(f.ToString());
                });
            }

            return View(names);
        }
        [HttpPost]
        public ActionResult Add(string name)
        {
            _db.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(string name)
        {
            _db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }
    }
}
