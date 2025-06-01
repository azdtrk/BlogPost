using Blog.Application.Repositories.User;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.User
{
    public class UserWriteRepository : WriteRepository<Domain.Entities.User>, IUserWriteRepository
    {
        public UserWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
