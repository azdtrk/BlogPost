using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Blog.Domain.Entities
{
    public class ApplicationUserRole : IdentityRole<Guid>
    {
        public ICollection<Endpoint> Endpoints { get; set; }

        public UserRole RoleType { get; set; }

        #region Navigation Property to DomainUser

        public Guid? DomainUserId { get; set; }

        public User? DomainUser { get; set; }

        #endregion
    }

    public enum UserRole
    {
        Author,
        Reader
    }
}
