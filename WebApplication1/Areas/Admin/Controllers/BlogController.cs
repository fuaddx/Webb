using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.BlogVm;

namespace WebApplication1.Areas.Admin.Controllers
{
   /* [Authorize]*/
    [Area("Admin")]
    public class BlogController : Controller
    {
        DataDbContext _db {  get; set; }
        IWebHostEnvironment _env;

        public BlogController(DataDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Blogs.Select(c=> new BlogListItemVm
            {
                Id = c.Id,
                Title = c.Title,
                UpdatedTime = c.UpdatedTime,
                CreatedTime = c.CreatedTime,
                ImageUrl = c.ImageUrl,
                Description = c.Description,
                Author = c.Author,
            }).ToListAsync());
        }
        public IActionResult Create() {
            
            return View();  
        }
        public IActionResult Cancel() {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult>Create(BlogCreateVm vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            string filename = null;
            if (vm.MainImage != null)
            {
                filename = Guid.NewGuid()+Path.GetExtension(vm.MainImage.FileName);
                using (Stream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets", "image", "stories", filename), FileMode.Create))
                {
                       await vm.MainImage.CopyToAsync(fs);
                }
            }
            Blog blog = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                Author = vm.Author,
                ImageUrl = filename
            };
            _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id) 
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            return View(new BlogUpdatedVm
            {
                Title = data.Title,
                Description = data.Description,
                Author = data.Author,
                ImageUrl = data.ImageUrl,
            });
        }
        [HttpPost]
        public async Task<IActionResult>Update(int? id,BlogUpdatedVm vm)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Author = vm.Author;
            data.Description = vm.Description;
            
             if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "image", "stories", data.ImageUrl)))
             {
                 System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "image", "stories", data.ImageUrl));
             }
             string filename = Guid.NewGuid() + Path.GetExtension(vm.MainImage.FileName);
             using (Stream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets", "image", "stories", filename), FileMode.Create))
             {
                 await vm.MainImage.CopyToAsync(fs);
             }
            data.ImageUrl = filename;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult>Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blogs.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

