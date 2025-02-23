using FluentValidation;

namespace Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostValidator : AbstractValidator<CreateBlogPostRequest>
    {
        public CreateBlogPostValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Content).NotEmpty().MinimumLength(500);

            RuleFor(x => x.Preface).NotEmpty().MinimumLength(300);
        }
    }
}