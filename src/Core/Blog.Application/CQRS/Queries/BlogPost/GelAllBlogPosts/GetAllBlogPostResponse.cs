
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Wrappers;

namespace Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostResponse : ServiceResponse<List<BlogPostListDto>>
    {

    }
}
