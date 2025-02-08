using FluentValidation;

namespace AzadTurkSln.Application.CQRS.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostValidator : AbstractValidator<DeleteBlogPostRequest>
    {
        public DeleteBlogPostValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
