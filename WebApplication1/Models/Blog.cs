namespace WebApplication1.Models
{
    public class Blog:BaseEntity
    {
        
        public string? Author { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

    }
}
