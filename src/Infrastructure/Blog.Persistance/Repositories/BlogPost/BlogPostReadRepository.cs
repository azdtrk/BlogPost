using Blog.Application.Repositories.BlogPost;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.BlogPost
{
    public class BlogPostReadRepository : ReadRepository<Domain.Entities.BlogPost>, IBlogPostReadRepository
    {
        public BlogPostReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
