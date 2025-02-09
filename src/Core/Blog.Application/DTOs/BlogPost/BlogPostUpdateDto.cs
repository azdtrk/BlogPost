using Blog.Domain.Entities;

namespace Blog.Application.DTOs.BlogPost
{
    public class BlogPostUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Preface { get; set; }
        public ICollection<Image> Images { get; set; }
        public Image ThumbNailImage { get; set; }
    }
}
