namespace Blog.Application.DTOs.Comment
{
    public class CommentDto
    {
        public Guid BlogpostId { get; set; }
        public string Content { get; set; }
    }
}
