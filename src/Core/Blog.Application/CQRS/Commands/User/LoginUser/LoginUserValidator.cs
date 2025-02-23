using FluentValidation;

namespace Blog.Application.CQRS.Commands.User.LoginUser
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.UserNameOrEmail)
                .NotEmpty().WithMessage("{PropertyName} must be provided for your request to be processed");

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(8).WithMessage("{PropertyName} should at least be {MinLength} characters");

        }
    }
}