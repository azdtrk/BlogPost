using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostRequest : IRequest<ServiceResponse<GetCommentByBlogPostResponse>>
    {
    }
}
