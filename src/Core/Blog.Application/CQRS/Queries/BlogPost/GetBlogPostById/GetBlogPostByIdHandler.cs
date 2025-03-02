using AutoMapper;
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using Blog.Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Queries.BlogPost.GetBlogPostById
{
    public class GetBlogPostByIdHandler : IRequestHandler<GetBlogPostByIdRequest, GetBlogPostByIdResponse>
    {
        private readonly IBlogPostReadRepository _blogPostReadRpository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetBlogPostByIdRequest> _validator;
        private readonly ILogger<GetBlogPostByIdHandler> _logger;

        public GetBlogPostByIdHandler(
            IBlogPostReadRepository blogPostReadRpository,
            IMapper mapper,
            IValidator<GetBlogPostByIdRequest> validator,
            ILogger<GetBlogPostByIdHandler> logger)
        {
            _blogPostReadRpository = blogPostReadRpository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetBlogPostByIdResponse> Handle(GetBlogPostByIdRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Get Blog Post by Id request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                var blogPost = await _blogPostReadRpository.GetByIdAsync(request.Id);

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