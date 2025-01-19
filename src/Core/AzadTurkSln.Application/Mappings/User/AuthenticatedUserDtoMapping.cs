using AutoMapper;
using AzadTurkSln.Application.DTOs.User;

namespace AzadTurkSln.Application.Mappings.User
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
