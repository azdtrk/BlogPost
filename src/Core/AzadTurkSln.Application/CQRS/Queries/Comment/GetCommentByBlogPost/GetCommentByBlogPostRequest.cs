using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostRequest : IRequest<ServiceResponse<GetCommentByBlogPostResponse>>
    {
    }
}
