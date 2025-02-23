using MediatR;

namespace Blog.Application.CQRS.Commands.Comment.DeleteComment
{
    public class DeleteCommentRequest : IRequest<DeleteCommentResponse>
    {
        public Guid Id { get; set; }
    }
}
