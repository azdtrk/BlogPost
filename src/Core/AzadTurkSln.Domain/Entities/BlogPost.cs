using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzadTurkSln.Domain.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } = new DateTime();

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public bool CanBePublished { get; set; } = false;

        public DateTime DateUpdated { get; set; } = new DateTime();

        #region Navigation Properties

        public int AuthorId { get; set; }
        public User Author { get; set; } = new User();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        #endregion
    }
}
