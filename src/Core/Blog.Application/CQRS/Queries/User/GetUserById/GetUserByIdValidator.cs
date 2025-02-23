using FluentValidation;

namespace Blog.Application.CQRS.Queries.User.GetUserById
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequest>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} should be provided.");
        }
    }
}
