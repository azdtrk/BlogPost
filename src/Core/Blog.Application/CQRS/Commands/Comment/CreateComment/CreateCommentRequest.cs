using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Commands.Comment.CreateComment
{
    public class CreateCommentRequest : IRequest<ServiceResponse<CreateCommentResponse>>
    {
        public string Content { get; set; }
    }
}
