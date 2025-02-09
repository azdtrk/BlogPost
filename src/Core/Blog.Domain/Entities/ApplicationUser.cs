using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }

        #region Navigation Property to DomainUser

        public Guid DomainUserId { get; set; }

        public User? DomainUser { get; set; }
        #endregion
    }
}
