using Blog.Application.Repositories;
using Blog.Persistance.Context;
using Blog.Domain.Entities;

namespace Blog.Persistance.Repositories
{
    public class CommentReadRepository : ReadRepository<Comment>, ICommentReadRepository
    {
        public CommentReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
