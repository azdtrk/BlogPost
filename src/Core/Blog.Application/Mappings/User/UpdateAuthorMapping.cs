using AutoMapper;
using Blog.Application.CQRS.Commands.Author.UpdateAuthor;
using Blog.Application.DTOs.ImageDtos;

namespace Blog.Application.Mappings.User
{
    public class UpdateAuthorMapping : Profile
    {
        public UpdateAuthorMapping()
        {
            // Map from ImageDto to Image entity
            CreateMap<ImageDto, Domain.Entities.Image>();
            
            // Map from UpdateUserRequest to User entity
            CreateMap<UpdateAuthorRequest, Domain.Entities.Author>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
                .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.ProfilePhoto));
        }
    }
} 