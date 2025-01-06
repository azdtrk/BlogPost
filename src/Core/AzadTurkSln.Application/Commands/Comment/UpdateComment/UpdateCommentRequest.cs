using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Commands.Comment.UpdateComment
{
    public class UpdateCommentRequest : IRequest<ServiceResponse<UpdateCommentResponse>>
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
