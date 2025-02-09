using Blog.Application.Repositories;
using Blog.Persistance.Context;
using Blog.Domain.Entities;

namespace Blog.Persistance.Repositories
{
    public class BlogPostWriteRepository : WriteRepository<BlogPost>, IBlogPostWriteRepository
    {
        public BlogPostWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
