using FluentValidation;

namespace Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostValidator : AbstractValidator<CreateBlogPostRequest>
    {
        public CreateBlogPostValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("{PropertyName} should at least be {MinLength} characters");

            RuleFor(x => x.Content)
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("{PropertyName} should at least be {MinLength} characters");

            RuleFor(x => x.Preface)
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("{PropertyName} should at least be {MinLength} characters");

            RuleFor(x => x.ThumbNailImage).NotNull().NotEmpty();
        }
    }
}