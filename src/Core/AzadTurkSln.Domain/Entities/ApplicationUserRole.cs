using Microsoft.AspNetCore.Identity;

namespace AzadTurkSln.Domain.Entities
{
    public class ApplicationUserRole : IdentityRole<Guid>
    {
        public UserRole RoleType { get; set; }

        #region Navigation Property to DomainUser

        public Guid DomainUserId { get; set; }

        public User? DomainUser { get; set; }

        #endregion
    }

    public enum UserRole
    {
        Author,
        Reader
    }
}
