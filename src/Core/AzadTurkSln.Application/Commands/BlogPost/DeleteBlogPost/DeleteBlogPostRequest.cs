using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostRequest : IRequest<ServiceResponse<DeleteBlogPostResponse>>
    {
        public int Id { get; set; }
    }
}
