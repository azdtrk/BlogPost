using MediatR;

namespace Blog.Application.CQRS.Queries.Comment.GetAllComments
{
    public class GetAllCommentsRequest : IRequest<GetAllCommentsResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
