using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.Comment.UpdateComment
{
    public class UpdateCommentRequest : IRequest<UpdateCommentResponse>
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}
