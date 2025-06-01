using AutoMapper;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.BlogPost;
using MediatR;

namespace Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostHandler : IRequestHandler<CreateBlogPostRequest, CreateBlogPostResponse>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly IMapper _mapper;

        public CreateBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            IMapper mapper
        )
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _mapper = mapper;
        }

        public async Task<CreateBlogPostResponse> Handle(CreateBlogPostRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var blogPost = _mapper.Map<Domain.Entities.BlogPost>(request);

                if (blogPost == null)
                    throw new MappingException();

                await _blogPostWriteRepository.AddAsync(blogPost);

                var response = new CreateBlogPostResponse
                {
                    Value = $"Blogpost '({request.Title})' has been created",
                    CreatedDate = blogPost.DateCreated
                };
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error creating blogpost: ({ex.Message})");
            }
        }
    }
}