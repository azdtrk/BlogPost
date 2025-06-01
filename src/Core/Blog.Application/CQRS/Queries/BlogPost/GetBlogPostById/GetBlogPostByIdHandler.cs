using AutoMapper;
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.BlogPost;
using MediatR;

namespace Blog.Application.CQRS.Queries.BlogPost.GetBlogPostById
{
    public class GetBlogPostByIdHandler : IRequestHandler<GetBlogPostByIdRequest, GetBlogPostByIdResponse>
    {
        private readonly IBlogPostReadRepository _blogPostReadRepository;
        private readonly IMapper _mapper;

        public GetBlogPostByIdHandler(
            IBlogPostReadRepository blogPostReadRpository,
            IMapper mapper)
        {
            _blogPostReadRepository = blogPostReadRpository;
            _mapper = mapper;
        }

        public async Task<GetBlogPostByIdResponse> Handle(GetBlogPostByIdRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var blogPost = await _blogPostReadRepository.GetByIdAsync(request.Id);

                if (blogPost == null)
                    throw new EntityNotFoundException(nameof(BlogPost), request.Id);

                var blogPostDto = _mapper.Map<BlogPostSingleDto>(blogPost);

                if (blogPostDto == null)
                    throw new MappingException();

                var response = new GetBlogPostByIdResponse()
                {
                    Value = blogPostDto
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An error occured while getting the blog post: ({ex.Message})");
            }
        }
    }
}