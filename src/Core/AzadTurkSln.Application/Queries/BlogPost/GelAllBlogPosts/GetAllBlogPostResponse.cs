
using AzadTurkSln.Application.DTOs.BlogPost;

namespace AzadTurkSln.Application.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostResponse
    {
        public List<BlogPostListDto>? BlogPosts { get; set; }
    }
}
