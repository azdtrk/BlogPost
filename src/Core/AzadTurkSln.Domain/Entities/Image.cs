namespace AzadTurkSln.Domain.Entities
{
    public class Image : File
    {
        #region Navigation Properties

        public int? BlogPostId { get; set; }
        public BlogPost? BlogPost { get; set; }

        public BlogPost? ThumbnailForBlogPost { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        #endregion
    }
}
