using AutoMapper;
using Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost;
using Blog.Application.DTOs.ImageDtos;

namespace Blog.Application.Mappings.BlogPost
{
    public class CreateBlogPostMapping : Profile
    {
        public CreateBlogPostMapping()
        {
            // Map from request DTO to domain entity
            CreateMap<CreateBlogPostRequest, Domain.Entities.BlogPost>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CanBePublished, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => 0));

            // Map from ImageDto to Image entity
            CreateMap<ImageDto, Domain.Entities.Image>();
        }
    }
} 