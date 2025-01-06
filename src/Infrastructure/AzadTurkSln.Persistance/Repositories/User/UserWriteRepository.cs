using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Persistance.Repositories
{
    public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
    {
        public UserWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
