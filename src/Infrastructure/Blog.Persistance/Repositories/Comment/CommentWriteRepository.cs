using Blog.Application.Repositories;
using Blog.Persistance.Context;
using Blog.Domain.Entities;

namespace Blog.Persistance.Repositories
{
    public class CommentWriteRepository : WriteRepository<Comment>, ICommentWriteRepository
    {
        public CommentWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
