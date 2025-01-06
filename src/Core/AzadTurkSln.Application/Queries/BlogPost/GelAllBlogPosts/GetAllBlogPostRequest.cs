using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostRequest : IRequest<ServiceResponse<GetAllBlogPostResponse>>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
