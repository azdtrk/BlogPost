using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Queries.BlogPost.GetBlogPostById
{
    public class GetBlogPostByIdRequest : IRequest<GetBlogPostByIdResponse>
    {
        public Guid Id { get; set; }
    }
}
