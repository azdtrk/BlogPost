using FluentValidation;
using FluentValidation.Validators;

namespace AzadTurkSln.Application.CQRS.Commands.User.LoginUser
{
    public class RegisterUserValidator : AbstractValidator<LoginUserRequest>
    {
        public RegisterUserValidator()
        {
            
            RuleFor(u => u.Email)
                .NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email address");

            RuleFor(u => u.Password)
                .NotNull()
                .MinimumLength(5).WithMessage("{PropertyName} should at least be {MinLength} characters");

        }
    }
}