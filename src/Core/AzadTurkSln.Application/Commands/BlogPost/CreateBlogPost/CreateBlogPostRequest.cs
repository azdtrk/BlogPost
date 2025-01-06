using AzadTurkSln.Application.Wrappers;
using AzadTurkSln.Domain.Entities;
using MediatR;

namespace AzadTurkSln.Application.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostRequest : IRequest<ServiceResponse<CreateBlogPostResponse>>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Preface { get; set; } = string.Empty;
        public ICollection<Image> Images { get; set; }
        public Image ThumbNailImage { get; set; }
    }
}
