using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Persistance.Repositories
{
    public class CommentWriteRepository : WriteRepository<Comment>, ICommentWriteRepository
    {
        public CommentWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
