using Blog.Application.Wrappers;

namespace Blog.Application.CQRS.Commands.Comment.CreateComment
{
    public class CreateCommentResponse : ServiceResponse<string>
    {
        public DateTime CreatedDate { get; set; }
    }
}
