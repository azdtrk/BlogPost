using Blog.Application.Repositories.Author;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Author
{
    public class AuthorWriteRepository : WriteRepository<Domain.Entities.Author>, IAuthorWriteRepository
    {
        public AuthorWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}