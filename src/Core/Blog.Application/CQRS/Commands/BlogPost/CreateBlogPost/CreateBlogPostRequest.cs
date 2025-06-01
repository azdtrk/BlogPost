using Blog.Application.DTOs.ImageDtos;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostRequest : IRequest<CreateBlogPostResponse>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Preface { get; set; } = string.Empty;
        public ICollection<ImageDto>? Images { get; set; }
        public ImageDto ThumbNailImage { get; set; }
    }
}
