using Blog.Application.Repositories;
using Blog.Persistance.Context;
using Blog.Domain.Entities;

namespace Blog.Persistance.Repositories
{
    public class BlogPostReadRepository : ReadRepository<BlogPost>, IBlogPostReadRepository
    {
        public BlogPostReadRepository(ApplicationDbContext context) : base(context) { }

    }
}
