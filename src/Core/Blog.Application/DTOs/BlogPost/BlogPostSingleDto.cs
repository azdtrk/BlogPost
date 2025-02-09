using Blog.Domain.Entities;

namespace Blog.Application.DTOs.BlogPost
{
    public class BlogPostSingleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
