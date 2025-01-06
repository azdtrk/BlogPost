using FluentValidation;

namespace AzadTurkSln.Application.Commands.Comment.CreateComment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
