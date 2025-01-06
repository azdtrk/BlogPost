using AzadTurkSln.Application.Wrappers;
using AzadTurkSln.Domain.Entities;
using MediatR;

namespace AzadTurkSln.Application.Commands.BlogPost.UpdateBlogPost
{
    public class UpdateBlogPostRequest : IRequest<ServiceResponse<UpdateBlogPostResponse>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Preface { get; set; }
        public ICollection<Image> Images { get; set; }
        public Image ThumbNailImage { get; set; }
    }
}
