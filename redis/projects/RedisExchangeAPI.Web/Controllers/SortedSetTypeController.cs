using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Models;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class SortedSetTypeController : Controller
{
    private readonly RedisService _redis;
    private readonly IDatabase _db;
    private string listKey = "sortedsetnames";
    public SortedSetTypeController(RedisService rediService)
    {
        _redis = rediService;
        _db = _redis.GetDb(3);
    }
    public IActionResult Index()
    {
        HashSet<SortedSetModel> names = new HashSet<SortedSetModel>();

        if (_db.KeyExists(listKey))
        {
            _db.SortedSetScan(listKey).ToList().ForEach(x =>
            {
                names.Add(new SortedSetModel(x.Element, x.Score));
            });

            //score'a göre sıralayıp getirme
            // _db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
            // {
            //     names.Add(x.ToString());
            // });
        }

        return View(names);
    }
    [HttpPost]
    public IActionResult Add(string name, int score)
    {
        _db.SortedSetAdd(listKey, name, score);

        _db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Remove(string name)
    {
        await _db.SortedSetRemoveAsync(listKey, name);
        return RedirectToAction("Index");
    }
}
// StackExchange.Redis'te Sliding Expiration yok, sadece Absolute Expiration var.