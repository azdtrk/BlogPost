using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Queries.BlogPost.GetBlogPostById
{
    public class GetBlogPostByIdRequest : IRequest<GetBlogPostByIdResponse>
    {
        public Guid Id { get; set; }
    }
}
