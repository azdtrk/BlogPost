using FluentValidation;

namespace Blog.Application.CQRS.Commands.BlogPost.UploadImage
{
    public class UploadImageValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageValidator()
        {
            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");

            RuleFor(x => x.Image.ContentType)
                .Must(x => x.Contains("image/")).WithMessage("File must be an image.")
                .When(x => x.Image != null);
                
            RuleFor(x => x.Image.FileName)
                .Must(x => {
                    var extension = System.IO.Path.GetExtension(x).ToLowerInvariant();
                    return new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(extension);
                })
                .WithMessage("Invalid image format. Allowed formats: jpg, jpeg, png, gif, webp")
                .When(x => x.Image != null && !string.IsNullOrEmpty(x.Image.FileName));
        }
    }
} 