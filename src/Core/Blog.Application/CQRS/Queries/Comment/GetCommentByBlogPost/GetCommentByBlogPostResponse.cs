using Blog.Application.DTOs.Comment;
using Blog.Application.Wrappers;

namespace Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostResponse : ServiceResponse<List<CommentDto>>
    {

    }
}
