using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace MyApp.Namespace;

public class SetTypeController : Controller
{
    private readonly RedisService _redis;
    private readonly IDatabase _db;
    private string listKey = "hashnames";
    public SetTypeController(RedisService rediService)
    {
        _redis = rediService;
        _db = _redis.GetDb(2);
    }
    public IActionResult Index()
    {
        HashSet<string> names = new HashSet<string>();

        if (_db.KeyExists(listKey))
        {
            _db.SetMembers(listKey).ToList().ForEach(x =>
            {
                names.Add(x.ToString());
            });
        }

        return View(names);
    }
    [HttpPost]
    public IActionResult Add(string name)
    {
        // if (!_db.KeyExists(listKey))
        // {
        _db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
        // }

        _db.SetAdd(listKey, name);

        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Remove(string name)
    {
        await _db.SetRemoveAsync(listKey, name);
        return RedirectToAction("Index");
    }
}

// StackExchange.Redis'te Sliding Expiration yok, sadece Absolute Expiration var.