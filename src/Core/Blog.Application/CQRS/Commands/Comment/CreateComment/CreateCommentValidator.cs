using FluentValidation;

namespace Blog.Application.CQRS.Commands.Comment.CreateComment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} required.");

            RuleFor(x => x.BlogPostId)
                .NotEmpty().WithMessage("{PropertyName} required.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(200).WithMessage("{PropertyName} cannot be longer than {MaxLength}.");
        }
    }
}
