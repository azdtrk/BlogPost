using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Queries.Comment.GetCommentById
{
    public class GetCommentByBlogPostRequest : IRequest<ServiceResponse<GetCommentByBlogPostResponse>>
    {
    }
}
