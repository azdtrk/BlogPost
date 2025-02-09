using MediatR;

namespace Blog.Application.CQRS.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostRequest : IRequest<DeleteBlogPostResponse>
    {
        public Guid Id { get; set; }
    }
}
