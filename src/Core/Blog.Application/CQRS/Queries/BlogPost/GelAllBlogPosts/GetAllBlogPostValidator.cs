using FluentValidation;

namespace Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostValidator : AbstractValidator<GetAllBlogPostRequest>
    {
        public GetAllBlogPostValidator()
        {

            RuleFor(u => u.Page)
                .LessThan(0).WithMessage("{PropertyName} should at least be starting from 0")
                .GreaterThan(5).WithMessage("{PropertyName} cannot be greater then 5");

            RuleFor(u => u.Size)
                .LessThan(0).WithMessage("{PropertyName} should at least be starting from 0")
                .GreaterThan(10).WithMessage("{PropertyName} cannot be greater then 5");

        }

    }
}
