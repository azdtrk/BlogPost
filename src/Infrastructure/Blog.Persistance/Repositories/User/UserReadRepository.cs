using Blog.Application.Repositories;
using Blog.Persistance.Context;
using Blog.Domain.Entities;

namespace Blog.Persistance.Repositories
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
