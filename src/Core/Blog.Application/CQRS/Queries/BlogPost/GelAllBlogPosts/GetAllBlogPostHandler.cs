using AutoMapper;
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using Blog.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostHandler : IRequestHandler<GetAllBlogPostRequest, GetAllBlogPostResponse>
    {
        private readonly IBlogPostReadRepository _blogPostReadRpository;
        private readonly IMapper _mapper;

        public GetAllBlogPostHandler(IBlogPostReadRepository blogPostReadRpository, IMapper mapper)
        {
            _blogPostReadRpository = blogPostReadRpository;
            _mapper = mapper;
        }

        public async Task<GetAllBlogPostResponse> Handle(GetAllBlogPostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var blogPosts = _blogPostReadRpository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                    .Include(bp => bp.ThumbnailImage)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var blogPostListDto = _mapper.Map<List<BlogPostListDto>>(blogPosts);

                if (blogPostListDto == null)
                    throw new MappingException();

                var response = new GetAllBlogPostResponse()
                {
                    Value = blogPostListDto
                };
                
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An error occured while getting blog posts: ({ex.Message})");
            }
        }
    }
}
