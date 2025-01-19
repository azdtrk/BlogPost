using AutoMapper;
using AzadTurkSln.Application.DTOs.BlogPost;
using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AzadTurkSln.Application.Queries.BlogPost.GelAllBlogPosts
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
