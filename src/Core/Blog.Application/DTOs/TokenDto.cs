namespace Blog.Application.DTOs
{
    public class TokenDto
    {
        public required string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public required string RefreshToken { get; set; }

    }
}
