using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public class Endpoint : BaseEntity
    {
        
        public string ActionType { get; set; }
        public string HttpType { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }
        public ICollection<ApplicationUserRole> Roles { get; set; }
        
        public Endpoint()
        {
            Roles = new HashSet<ApplicationUserRole>();
        }

    }
}
