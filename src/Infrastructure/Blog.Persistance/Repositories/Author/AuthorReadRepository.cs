using Blog.Application.Repositories.Author;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Author
{
    public class AuthorReadRepository : ReadRepository<Domain.Entities.Author>, IAuthorReadRepository
    {
        public AuthorReadRepository(ApplicationDbContext context) : base(context) { }
    }
}