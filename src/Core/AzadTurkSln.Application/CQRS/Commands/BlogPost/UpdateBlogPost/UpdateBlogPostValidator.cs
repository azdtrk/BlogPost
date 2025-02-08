using FluentValidation;

namespace AzadTurkSln.Application.CQRS.Commands.BlogPost.UpdateBlogPost
{
    public class UpdateBlogPostValidator : AbstractValidator<UpdateBlogPostRequest>
    {
        public UpdateBlogPostValidator()
        {
            RuleFor(x => x.Title).MaximumLength(200);

            RuleFor(x => x.Content).MinimumLength(500);

            RuleFor(x => x.Preface).MinimumLength(300);
        }
    }
}
