using FluentValidation;

namespace Blog.Application.CQRS.Commands.User.UpdatePassword
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(u => u.Password)
                .NotNull().WithMessage("{PropertyName} cannot be empty")
                .Equal(u => u.PasswordConfirm).WithMessage("Password should match!")
                .MinimumLength(8).WithMessage("{PropertyName} should at least be {MinLength} characters");

            RuleFor(u => u.PasswordConfirm)
                .NotNull().WithMessage("{PropertyName} cannot be empty")
                .Equal(u => u.Password).WithMessage("Password should match!");
        }
    }
}
