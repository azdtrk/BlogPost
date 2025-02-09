namespace Blog.Application.DTOs.BlogPost
{
    public class BlogPostListDto
    {
        public string Title { get; set; } = string.Empty;
        public string Preface { get; set; } = string.Empty;
        public string? ThumbnailImageUrl { get; set; }

    }
}
