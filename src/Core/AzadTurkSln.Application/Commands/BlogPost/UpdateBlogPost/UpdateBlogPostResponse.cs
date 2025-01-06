using AzadTurkSln.Application.DTOs.BlogPost;
using AzadTurkSln.Application.DTOs.Comment;

namespace AzadTurkSln.Application.Commands.BlogPost.UpdateBlogPost
{
    public class UpdateBlogPostResponse
    {
        public BlogPostUpdateDto updatedBlogPost { get; set; }
    }
}
