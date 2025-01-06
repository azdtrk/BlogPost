using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Commands.Comment.CreateComment
{
    public class CreateCommentRequest : IRequest<ServiceResponse<CreateCommentResponse>>
    {
        public string Content { get; set; }
    }
}
