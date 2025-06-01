namespace Blog.Domain.Entities
{
    public class Image : File
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsThumbnail { get; set; }

        #region Navigation Properties

        public Guid? BlogPostId { get; set; }
        public BlogPost? BlogPost { get; set; }

        public Guid? ThumbnailForBlogPostId { get; set; }
        public BlogPost? ThumbnailForBlogPost { get; set; }

        public Guid? AuthorId { get; set; }
        public Author? Author { get; set; }

        #endregion
    }
}
