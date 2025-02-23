using MediatR;

namespace Blog.Application.CQRS.Commands.Comment.UpdateComment
{
    public class UpdateCommentRequest : IRequest<UpdateCommentResponse>
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
