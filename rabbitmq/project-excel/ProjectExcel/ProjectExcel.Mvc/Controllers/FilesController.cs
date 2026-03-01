using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectExcel.Mvc.Hubs;
using ProjectExcel.Mvc.Models;

namespace ProjectExcel.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<MyHub> _hub;

        public FilesController(AppDbContext context, IHubContext<MyHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Devamke");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file is not { Length: > 0 }) return BadRequest();

            var userFile = await _context.UserFiles.FirstAsync(x => x.Id == fileId);

            var filePath = userFile.FileName + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", filePath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            userFile.CreatedDate = DateTime.Now;
            userFile.FilePath = filePath;
            userFile.FileStatus = FileStatus.Completed;

            await _context.SaveChangesAsync();

            await _hub.Clients.User(userFile.UserId).SendAsync("CompletedFile");

            return Ok();
        }
    }
}
