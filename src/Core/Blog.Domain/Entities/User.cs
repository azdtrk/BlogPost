using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? About { get; set; }


        #region Navigation Properties

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public Guid ApplicationUserRoleId { get; set; }
        public ApplicationUserRole? ApplicationUserRole { get; set; }

        public List<Comment>? Comments { get; set; }

        public List<BlogPost>? BlogPosts { get; set; }

        public Guid? ProfilePhotoId { get; set; }
        public Image? ProfilePhoto { get; set; }

        #endregion
    }
    
}
