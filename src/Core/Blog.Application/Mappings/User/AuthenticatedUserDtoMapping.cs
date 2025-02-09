using AutoMapper;
using Blog.Application.DTOs.User;

namespace Blog.Application.Mappings.User
{
    public class AuthenticatedUserDtoMapping : Profile
    {
        public AuthenticatedUserDtoMapping()
        {
            CreateMap<Domain.Entities.User, AuthenticatedUserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name != null ? src.Name : null))
                .ReverseMap();
        }
    }
}
