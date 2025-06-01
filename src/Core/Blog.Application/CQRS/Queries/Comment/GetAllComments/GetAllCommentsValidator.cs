using FluentValidation;

namespace Blog.Application.CQRS.Queries.Comment.GetAllComments
{
    public class GetAllCommentsValidator : AbstractValidator<GetAllCommentsRequest>
    {
        public GetAllCommentsValidator()
        {
            RuleFor(u => u.Page)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} should at least be starting from 0")
                .LessThan(5).WithMessage("{PropertyName} cannot be greater then 5");

            RuleFor(u => u.Size)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} should at least be starting from 0")
                .LessThan(20).WithMessage("{PropertyName} cannot be greater then 20");
        }
    }
}
