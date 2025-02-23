using AutoMapper;
using Blog.Application.DTOs.BlogPost;

namespace Blog.Application.Mappings.BlogPost
{
    public class UpdateBlogPostDtoMappingcs : Profile
    {
        public UpdateBlogPostDtoMappingcs()
        {
            CreateMap<Domain.Entities.BlogPost, BlogPostUpdateDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title != null ? src.Title : null))
                .ForMember(dest => dest.Preface, opt => opt.MapFrom(src => src.Preface != null ? src.Preface : null));

        }
    }
}
