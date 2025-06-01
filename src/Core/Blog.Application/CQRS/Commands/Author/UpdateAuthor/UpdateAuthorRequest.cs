using Blog.Application.DTOs.ImageDtos;
using MediatR;

namespace Blog.Application.CQRS.Commands.Author.UpdateAuthor
{
    public class UpdateAuthorRequest : IRequest<UpdateAuthorResponse>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? About { get; set; }
        public ImageDto? ProfilePhoto { get; set; }
    }
}
