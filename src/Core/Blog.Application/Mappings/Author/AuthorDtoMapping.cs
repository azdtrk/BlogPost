using AutoMapper;
using Blog.Application.CQRS.Commands.Author.RegisterAuthor;
using Blog.Application.CQRS.Commands.Author.UpdateAuthor;
using Blog.Application.DTOs.User;

namespace Blog.Application.Mappings.Author
{
    public class AuthorDtoMapping : Profile
    {
        public AuthorDtoMapping()
        {
            // Map Author entity to AuthorDto
            CreateMap<Domain.Entities.Author, AuthorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
                .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.ProfilePhoto));

            // Map RegisterAuthorRequest to Author entity
            CreateMap<RegisterAuthorRequest, Domain.Entities.Author>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore());

            // Map UpdateAuthorRequest to Author entity
            CreateMap<UpdateAuthorRequest, Domain.Entities.Author>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
                .ForMember(dest => dest.ProfilePhoto, opt => opt.Ignore());
        }
    }
} 