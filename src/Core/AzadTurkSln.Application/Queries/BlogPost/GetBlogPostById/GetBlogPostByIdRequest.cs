using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Queries.BlogPost.GetBlogPostById
{
    public class GetBlogPostByIdRequest : IRequest<ServiceResponse<GetBlogPostByIdResponse>>
    {
        public int Id { get; set; }
    }
}
