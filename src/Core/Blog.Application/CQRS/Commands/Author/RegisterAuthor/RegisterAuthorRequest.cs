using Blog.Application.DTOs.ImageDtos;
using MediatR;

namespace Blog.Application.CQRS.Commands.Author.RegisterAuthor
{
    public class RegisterAuthorRequest : IRequest<RegisterAuthorResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? About { get; set; }
        public ImageDto? ProfilePhoto { get; set; }
    }
}
