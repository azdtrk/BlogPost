using FluentValidation;

namespace AzadTurkSln.Application.Commands.Comment.UpdateComment
{
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Content).NotEmpty().MaximumLength(200);
        }
    }
}
