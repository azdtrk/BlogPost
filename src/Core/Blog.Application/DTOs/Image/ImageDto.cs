namespace Blog.Application.DTOs.ImageDtos
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public string Path { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string? Alt { get; set; }
        public bool IsThumbnail { get; set; }
        public Guid? BlogPostId { get; set; }
        public Guid? ThumbnailForBlogPostId { get; set; }
        public DateTime UploadedAt { get; set; }
    }
} 