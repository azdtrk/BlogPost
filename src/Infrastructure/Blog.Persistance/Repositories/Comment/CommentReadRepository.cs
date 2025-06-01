using Blog.Application.Repositories.Comment;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Comment
{
    public class CommentReadRepository : ReadRepository<Domain.Entities.Comment>, ICommentReadRepository
    {
        public CommentReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
