using FluentValidation;

namespace AzadTurkSln.Application.CQRS.Commands.Comment.DeleteComment
{
    public class DeleteCommentValidator : AbstractValidator<DeleteCommentRequest>
    {
        public DeleteCommentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
