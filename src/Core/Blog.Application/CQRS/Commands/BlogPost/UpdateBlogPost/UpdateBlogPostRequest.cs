using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.CQRS.Commands.BlogPost.UpdateBlogPost
{
    public class UpdateBlogPostRequest : IRequest<UpdateBlogPostResponse>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Preface { get; set; }
        public ICollection<Image>? Images { get; set; }
        public Image? ThumbNailImage { get; set; }
    }
}
