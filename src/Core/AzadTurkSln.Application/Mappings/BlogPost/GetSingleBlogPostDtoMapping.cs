using AutoMapper;
using AzadTurkSln.Application.DTOs.BlogPost;

namespace AzadTurkSln.Application.Mappings.BlogPost
{
    public class GetSingleBlogPostDtoMapping : Profile
    {
        public GetSingleBlogPostDtoMapping()
        {
            CreateMap<Domain.Entities.BlogPost, BlogPostSingleDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title : null))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content != null ? src.Content : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images != null ? src.Images : null));

        }
    }
}
