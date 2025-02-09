using Blog.Domain.Entities;

namespace Blog.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string About { get; set; }
        public Image ProfilePhoto { get; set; }
    }
}
