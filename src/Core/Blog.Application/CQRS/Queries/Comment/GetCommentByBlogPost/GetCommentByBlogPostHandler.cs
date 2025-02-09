using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostHandler : IRequestHandler<GetCommentByBlogPostRequest, ServiceResponse<GetCommentByBlogPostResponse>>
    {
        public Task<ServiceResponse<GetCommentByBlogPostResponse>> Handle(GetCommentByBlogPostRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
