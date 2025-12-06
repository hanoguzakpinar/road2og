using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistrubutedCacheRedisApp.Web.Controllers;

public class CarsController : Controller
{
    private readonly IDistributedCache _distributedCache;

    public CarsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<ActionResult> Index()
    {
        var cacheOpt = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(30)
        };

        var car = new Car
        {
            Id = 1,
            Name = "Peugeot 208",
            Price = 1000
        };

        var json = JsonSerializer.Serialize(car);

        await _distributedCache.SetStringAsync("product:1", json, cacheOpt);

        return Content(json);
    }

    public async Task<ActionResult> Show()
    {
        var car = await _distributedCache.GetStringAsync("product:1");

        Car c = JsonSerializer.Deserialize<Car>(car);

        return Content(string.Format($"Id:{c.Id} Name:{c.Name} Price:{c.Price}"));
    }

    public ActionResult Remove()
    {
        _distributedCache.Remove("product:1");
        return Content("Silindi.");
    }
}

