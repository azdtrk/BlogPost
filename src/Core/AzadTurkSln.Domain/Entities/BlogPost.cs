using AzadTurkSln.Domain.Common;

namespace AzadTurkSln.Domain.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Preface { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public bool CanBePublished { get; set; } = false;

        public DateTime DateUpdated { get; set; }

        public int LikeCount { get; set; }

        #region Navigation Properties

        public int AuthorId { get; set; }
        public User Author { get; set; } = new User();

        public ICollection<Comment>? Comments { get; set; }
        
        public ICollection<Image>? Images { get; set; }

        public int? ThumbnailImageId { get; set; }
        public Image? ThumbnailImage { get; set; }

        #endregion

        public BlogPost()
        {
            DateUpdated = DateTime.Now;
        }

    }
}
