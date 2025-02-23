using Blog.Application.Wrappers;

namespace Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostResponse : ServiceResponse<string>
    {
        public DateTime CreatedDate { get; set; }
    }
}
