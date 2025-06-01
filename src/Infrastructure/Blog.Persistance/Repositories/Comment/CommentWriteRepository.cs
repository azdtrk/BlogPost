using Blog.Application.Repositories.Comment;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Comment
{
    public class CommentWriteRepository : WriteRepository<Domain.Entities.Comment>, ICommentWriteRepository
    {
        public CommentWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
