using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Persistance.Repositories
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
