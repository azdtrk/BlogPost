using FluentValidation;

namespace AzadTurkSln.Application.CQRS.Commands.User.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .MinimumLength(5).WithMessage("{PropertyName} should at least be {MinLength} characters");

            RuleFor(u => u.Email)
                .NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email address");

            RuleFor(u => u.Password)
                .NotNull()
                .MinimumLength(8).WithMessage("{PropertyName} should at least be {MinLength} characters");

        }
    }
}