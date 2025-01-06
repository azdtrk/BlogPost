using FluentValidation;

namespace AzadTurkSln.Application.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostValidator : AbstractValidator<DeleteBlogPostRequest>
    {
        public DeleteBlogPostValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
