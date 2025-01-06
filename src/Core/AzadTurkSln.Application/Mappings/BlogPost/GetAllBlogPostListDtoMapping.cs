using AutoMapper;
using AzadTurkSln.Application.DTOs.BlogPost;

namespace AzadTurkSln.Application.Mappings.BlogPost
{
    public class GetAllBlogPostListDtoMapping : Profile
    {
        public GetAllBlogPostListDtoMapping()
        {
            CreateMap<Domain.Entities.BlogPost, BlogPostListDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title : null))
                .ForMember(dest => dest.Preface, opt => opt.MapFrom(src => src.Preface != null ? src.Preface : null))
                .ForMember(dest => dest.ThumbnailImageUrl, opt => opt.MapFrom(src => src.ThumbnailImage != null ? src.ThumbnailImage.Path : null));

        }
    }
}
