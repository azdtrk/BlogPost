using Blog.Application.Repositories;
using Blog.Persistance.Context;
using Blog.Domain.Entities;

namespace Blog.Persistance.Repositories
{
    public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
    {
        public UserWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
