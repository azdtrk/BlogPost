using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Queries.Comment.GetCommentById
{
    public class GetCommentByBlogPostHandler : IRequestHandler<GetCommentByBlogPostRequest, ServiceResponse<GetCommentByBlogPostResponse>>
    {
        public Task<ServiceResponse<GetCommentByBlogPostResponse>> Handle(GetCommentByBlogPostRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
