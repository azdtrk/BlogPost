using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public abstract class File : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Storage { get; set; } = "local"; // local, azure, aws, etc.
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
    }
}
