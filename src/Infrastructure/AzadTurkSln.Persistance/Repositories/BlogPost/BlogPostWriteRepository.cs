using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Persistance.Context;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Persistance.Repositories
{
    public class BlogPostWriteRepository : WriteRepository<BlogPost>, IBlogPostWriteRepository
    {
        public BlogPostWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
