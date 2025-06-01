using Blog.Domain.Entities;

namespace Blog.Application.DTOs.User
{
    public class AuthorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? About { get; set; }
        public Image? ProfilePhoto { get; set; }
    }
}
