namespace Blog.Application.DTOs.BlogPost
{
    public class BlogPostUpdateDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Preface { get; set; }
    }
}
