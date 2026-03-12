using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DockerMvc.Models;

namespace DockerMvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult ImageSave()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImageSave(IFormFile file)
    {
        if (file is { Length: > 0 })
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        return View();
    }
}
