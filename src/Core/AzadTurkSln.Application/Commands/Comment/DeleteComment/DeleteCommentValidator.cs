using FluentValidation;

namespace AzadTurkSln.Application.Commands.Comment.DeleteComment
{
    public class DeleteCommentValidator : AbstractValidator<DeleteCommentRequest>
    {
        public DeleteCommentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
