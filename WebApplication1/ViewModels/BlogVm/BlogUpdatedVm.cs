namespace WebApplication1.ViewModels.BlogVm
{
    public class BlogUpdatedVm
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public IFormFile? MainImage { get; set; }
    }
}
