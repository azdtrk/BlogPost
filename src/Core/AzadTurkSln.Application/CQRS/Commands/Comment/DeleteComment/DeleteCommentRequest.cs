using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.Comment.DeleteComment
{
    public class DeleteCommentRequest : IRequest<DeleteCommentResponse>
    {
        public Guid Id { get; set; }
    }
}
