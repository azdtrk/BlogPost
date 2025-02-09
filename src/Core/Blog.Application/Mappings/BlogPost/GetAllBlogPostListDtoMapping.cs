using AutoMapper;
using Blog.Application.DTOs.BlogPost;

namespace Blog.Application.Mappings.BlogPost
{
    public class GetAllBlogPostListDtoMapping : Profile
    {
        public GetAllBlogPostListDtoMapping()
        {
            CreateMap<Domain.Entities.BlogPost, BlogPostListDto>()
                .ForMember(dest => dest.Preface, opt => opt.MapFrom(src => src.Preface != null ? src.Preface : null))
                .ForMember(dest => dest.ThumbnailImageUrl, opt => opt.MapFrom(src => src.ThumbnailImage != null ? src.ThumbnailImage.Path : null));
        }
    }
}
