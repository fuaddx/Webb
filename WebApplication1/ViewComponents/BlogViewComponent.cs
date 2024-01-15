using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.BlogVm;

namespace WebApplication1.ViewComponents
{
    public class BlogViewComponent: ViewComponent
    {
        DataDbContext _db {  get; set; }
        
        public BlogViewComponent(DataDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View( await _db.Blogs.Select(c=> new BlogListItemVm
            {
                Id  =c.Id,
                Title = c.Title,
                Description = c.Description,
                Author =c.Author,
                ImageUrl =c.ImageUrl,
            }).ToListAsync());
        }
    }
}
