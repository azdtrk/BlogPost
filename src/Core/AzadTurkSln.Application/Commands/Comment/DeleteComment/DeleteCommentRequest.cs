using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Commands.Comment.DeleteComment
{
    public class DeleteCommentRequest : IRequest<ServiceResponse<DeleteCommentResponse>>
    {
        public int Id { get; set; }
    }
}
