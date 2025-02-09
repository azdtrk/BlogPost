namespace Blog.Domain.Entities
{
    public class Image : File
    {
        #region Navigation Properties

        public Guid? BlogPostId { get; set; }
        public BlogPost? BlogPost { get; set; }

        public BlogPost? ThumbnailForBlogPost { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        #endregion
    }
}
