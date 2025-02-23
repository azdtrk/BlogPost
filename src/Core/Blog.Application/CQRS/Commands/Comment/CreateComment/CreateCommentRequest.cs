using MediatR;

namespace Blog.Application.CQRS.Commands.Comment.CreateComment
{
    public class CreateCommentRequest : IRequest<CreateCommentResponse>
    {
        public Guid UserId { get; set; }
        public Guid BlogPostId { get; set; }
        public string Content { get; set; }
    }
}
