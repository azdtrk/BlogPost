using System.ComponentModel.DataAnnotations;

namespace AzadTurkSln.Domain.Entities
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } = new DateTime();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Reader;

        #region Navigation Properties

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

        #endregion
    }
    public enum UserRole
    {
        Author,
        Reader
    }
}
