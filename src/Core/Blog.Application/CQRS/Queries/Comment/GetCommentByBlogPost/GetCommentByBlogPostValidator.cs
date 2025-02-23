using FluentValidation;

namespace Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostValidator : AbstractValidator<GetCommentByBlogPostRequest>
    {
        public GetCommentByBlogPostValidator()
        {
            RuleFor(x => x.BlogPostId)
                .NotEmpty().WithMessage("{PropertyName} should be provided.");
        }
    }
}
