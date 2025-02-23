using AutoMapper;
using Blog.Application.DTOs.Comment;

namespace Blog.Application.Mappings.Comments
{
    public class GetAllBlogCommentsDtoMapping : Profile
    {
        public GetAllBlogCommentsDtoMapping()
        {
            CreateMap<Domain.Entities.Comment, CommentDto>()
                .ForMember(dest => dest.BlogpostId, opt => opt.MapFrom(src => src.BlogPost.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content != null ? src.Content : null));

        }
    }
}
