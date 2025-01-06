using AzadTurkSln.Domain.Common;

namespace AzadTurkSln.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? About { get; set; }

        public UserRole Role { get; set; } = UserRole.Reader;


        #region Navigation Properties

        public List<Comment>? Comments { get; set; }

        public List<BlogPost>? BlogPosts { get; set; }

        public int? ProfilePhotoId { get; set; }
        public Image? ProfilePhoto { get; set; }

        #endregion
    }
    public enum UserRole
    {
        Author,
        Reader
    }
}
