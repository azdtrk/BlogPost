using FluentValidation;

namespace Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostValidator : AbstractValidator<GetAllBlogPostRequest>
    {
        public GetAllBlogPostValidator()
        {
            RuleFor(u => u.Page)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} should at least be starting from 0")
                .LessThan(15).WithMessage("{PropertyName} cannot be greater then 15");

            RuleFor(u => u.Size)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} should at least be starting from 0")
                .LessThan(100).WithMessage("{PropertyName} cannot be greater then 100");
        }
    }
}
