using Blog.Application.Repositories.User;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.User
{
    public class UserReadRepository : ReadRepository<Domain.Entities.User>, IUserReadRepository
    {
        public UserReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
