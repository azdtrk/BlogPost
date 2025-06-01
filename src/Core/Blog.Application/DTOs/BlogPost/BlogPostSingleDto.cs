
namespace Blog.Application.DTOs.BlogPost
{
    public class BlogPostSingleDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        
        public ICollection<Domain.Entities.Comment> Comments { get; set; } = new List<Domain.Entities.Comment>();
        public ICollection<Domain.Entities.Image> Images { get; set; } = new List<Domain.Entities.Image>();
    }
}
