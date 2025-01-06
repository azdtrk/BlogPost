using AutoMapper;
using AzadTurkSln.Application.DTOs.BlogPost;

namespace AzadTurkSln.Application.Mappings.BlogPost
{
    public class UpdateBlogPostDtoMappingcs : Profile
    {
        public UpdateBlogPostDtoMappingcs()
        {
            CreateMap<Domain.Entities.BlogPost, BlogPostUpdateDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title : null))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content != null ? src.Content : null))
                .ForMember(dest => dest.Preface, opt => opt.MapFrom(src => src.Preface != null ? src.Preface : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images != null ? src.Images : null))
                .ForMember(dest => dest.ThumbNailImage, opt => opt.MapFrom(src => src.ThumbnailImage != null ? src.ThumbnailImage : null));
        }
    }
}
