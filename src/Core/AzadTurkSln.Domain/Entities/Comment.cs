using System.ComponentModel.DataAnnotations;

namespace AzadTurkSln.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } = new DateTime();

        public string Content { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;


        #region Navigation Properties

        public int UserId { get; set; }
        public User User { get; set; } = new User();

        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = new BlogPost();

        #endregion
    }
}
