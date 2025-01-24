using AzadTurkSln.Domain.Common;

namespace AzadTurkSln.Domain.Entities
{
    public class Endpoint : BaseEntity
    {
        public Endpoint()
        {
            Roles = new HashSet<ApplicationUserRole>();
        }
        public string ActionType { get; set; }
        public string HttpType { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }
        public ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
