using Blog.Application.DTOs.Comment;
using Blog.Application.Wrappers;

namespace Blog.Application.CQRS.Queries.Comment.GetAllComments
{
    public class GetAllCommentsResponse : ServiceResponse<List<CommentDto>>
    {

    }
}
