using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Models;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class HashTypeController : Controller
{
    private readonly RedisService _redis;
    private readonly IDatabase _db;
    private string listKey = "sozluk";
    public HashTypeController(RedisService rediService)
    {
        _redis = rediService;
        _db = _redis.GetDb(4);
    }
    public IActionResult Index()
    {
        Dictionary<string, string> list = new Dictionary<string, string>();

        if (_db.KeyExists(listKey))
        {
            _db.HashGetAll(listKey).ToList().ForEach(x =>
            {
                list.Add(x.Name.ToString(), x.Value.ToString());
            });
        }

        return View(list);
    }
    [HttpPost]
    public IActionResult Add(string name, int value)
    {
        _db.HashSet(listKey, name, value);

        _db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Remove(string name)
    {
        await _db.HashDeleteAsync(listKey, name);
        return RedirectToAction("Index");
    }
}