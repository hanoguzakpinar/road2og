using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistrubutedCacheRedisApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IDistributedCache _distributedCache;

    public ProductsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<ActionResult> Index()
    {
        _distributedCache.SetString("name", "Oğuz", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        });

        await _distributedCache.SetStringAsync("surname", "Akpınar", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1)
        });

        return Content("Veri cachelendi.");
    }

    public ActionResult Show()
    {
        var name = _distributedCache.GetString("name");
        var surname = _distributedCache.GetString("surname");

        return Content(string.Format($"{name} {surname}"));
    }

    public ActionResult Remove()
    {
        _distributedCache.Remove("name");
        return Content("Silindi.");
    }

    public ActionResult ImageCache()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatar.jpg");

        var imgBytes = System.IO.File.ReadAllBytes(path);

        _distributedCache.Set("resim", imgBytes);

        return Content("kaydedildi.");
    }

    public IActionResult ImageShow()
    {
        var cachedData = _distributedCache.Get("resim");

        return File(cachedData,"image/jpg");
    }
}

