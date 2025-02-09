using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostRequest : IRequest<GetAllBlogPostResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
