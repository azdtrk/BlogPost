using AzadTurkSln.Domain.Common;

namespace AzadTurkSln.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public bool IsApproved { get; set; }

        public Comment()
        {
            Content = string.Empty;
            IsApproved = false;
        }

        #region Navigation Properties

        public Guid UserId { get; set; }
        public User User { get; set; } = new User();

        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = new BlogPost();

        public Guid? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; } = new Comment();
        public ICollection<Comment>? Replies { get; set; } = new List<Comment>();

        #endregion

    }
}
