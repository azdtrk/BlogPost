using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostRequest : IRequest<DeleteBlogPostResponse>
    {
        public Guid Id { get; set; }
    }
}
