using FluentValidation;

namespace Blog.Application.CQRS.Commands.User.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(u => u.About)
                .MinimumLength(100).WithMessage("{PropertyName} should at least be {MinLength} characters");
        }
    }
}
