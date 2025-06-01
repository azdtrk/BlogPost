using Blog.Application.Repositories.BlogPost;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.BlogPost
{
    public class BlogPostWriteRepository : WriteRepository<Domain.Entities.BlogPost>, IBlogPostWriteRepository
    {
        public BlogPostWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
