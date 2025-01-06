using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Persistance.Repositories
{
    public class CommentReadRepository : ReadRepository<Comment>, ICommentReadRepository
    {
        public CommentReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
