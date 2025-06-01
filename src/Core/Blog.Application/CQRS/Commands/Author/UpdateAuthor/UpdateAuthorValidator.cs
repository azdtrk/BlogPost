using FluentValidation;

namespace Blog.Application.CQRS.Commands.Author.UpdateAuthor
{
    public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorValidator()
        {
            RuleFor(u => u.About)
                .MinimumLength(100).WithMessage("{PropertyName} should at least be {MinLength} characters");
        }
    }
}
