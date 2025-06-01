using FluentValidation;

namespace Blog.Application.CQRS.Commands.User.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.UserName)
                .NotNull().WithMessage("{PropertyName} is required.")
                .MinimumLength(5).WithMessage("{PropertyName} should at least be {MinLength} characters");

            RuleFor(u => u.Email)
                .NotNull().WithMessage("{PropertyName} is required.")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email address");

            RuleFor(u => u.Password)
                .NotNull().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} should at least be {MinLength} characters");

        }
    }
}