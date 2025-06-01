using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.CQRS.Commands.BlogPost.UploadImage
{
    public class UploadImageRequest : IRequest<UploadImageResponse>
    {
        public IFormFile Image { get; set; }
        public bool IsThumbnail { get; set; }
        public Guid? BlogPostId { get; set; }
        public Guid? ThumbnailForBlogPostId { get; set; }
    }
}