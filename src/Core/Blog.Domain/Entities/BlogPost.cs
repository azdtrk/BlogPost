using System.ComponentModel.DataAnnotations;
using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Preface { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public bool CanBePublished { get; set; }

        public DateTime DateUpdated { get; set; }

        public int LikeCount { get; set; }

        #region Navigation Properties

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        
        public ICollection<Image>? Images { get; set; }

        public Guid? ThumbnailImageId { get; set; }
        public Image? ThumbnailImage { get; set; }
        #endregion

        public BlogPost()
        {
            DateUpdated = DateTime.Now;
            Author = new Author();
            CanBePublished = false;
        }

    }
}
