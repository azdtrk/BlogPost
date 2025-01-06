using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Persistance.Repositories
{
    public class BlogPostReadRepository : ReadRepository<BlogPost>, IBlogPostReadRepository
    {
        public BlogPostReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
