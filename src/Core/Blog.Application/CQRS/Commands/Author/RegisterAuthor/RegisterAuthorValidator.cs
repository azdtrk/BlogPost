using FluentValidation;

namespace Blog.Application.CQRS.Commands.Author.RegisterAuthor
{
    public class RegisterAuthorValidator : AbstractValidator<RegisterAuthorRequest>
    {
        public RegisterAuthorValidator()
        {
            RuleFor(u => u.About)
                .MinimumLength(100).WithMessage("{PropertyName} should at least be {MinLength} characters");
        }
    }
}
