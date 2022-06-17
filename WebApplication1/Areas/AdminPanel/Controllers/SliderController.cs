using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private AppDbContext _db { get; }
        private IWebHostEnvironment _env { get; }

        public SliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_db.Slides);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slide slide)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            if (slide.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Max size of img must be less than 200kb.");
            }
            if (!slide.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Type of file must be image.");

            }
            var filename = Guid.NewGuid().ToString() + slide.Image.FileName;
            var resultPath = Path.Combine(_env.WebRootPath, "img", filename);
            using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
            {
                await slide.Image.CopyToAsync(fileStream);
            }
            slide.Url = await slide.Image.SaveFileAsync(_env.WebRootPath,"img");
            await _db.Slides.AddAsync(slide);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
