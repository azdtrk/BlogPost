using MediatR;

namespace Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostRequest : IRequest<GetCommentByBlogPostResponse>
    {
        public Guid BlogPostId { get; set; }
    }
}
