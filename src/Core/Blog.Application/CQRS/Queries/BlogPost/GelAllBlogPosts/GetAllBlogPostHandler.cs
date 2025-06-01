using AutoMapper;
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.BlogPost;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts
{
    public class GetAllBlogPostHandler : IRequestHandler<GetAllBlogPostRequest, GetAllBlogPostResponse>
    {
        private readonly IBlogPostReadRepository _blogPostReadRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAllBlogPostRequest> _validator;
        private readonly ILogger<GetAllBlogPostHandler> _logger;

        public GetAllBlogPostHandler(
            IBlogPostReadRepository blogPostReadRepository,
            IMapper mapper,
            IValidator<GetAllBlogPostRequest> validator,
            ILogger<GetAllBlogPostHandler> logger
        )
        {
            _blogPostReadRepository = blogPostReadRepository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetAllBlogPostResponse> Handle(GetAllBlogPostRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Get all blogpost validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                var query = _blogPostReadRepository.GetAll(false);
                
                if (request.AuthorId.HasValue && request.AuthorId != Guid.Empty)
                {
                    // If viewing by author, show all their posts including drafts
                    query = query.Where(bp => bp.AuthorId == request.AuthorId.Value);
                }
                else
                {
                    // Only return published posts to the public
                    query = query.Where(bp => bp.CanBePublished == true);
                }
                
                // Apply pagination and include necessary relationships
                var blogPosts = await query
                    .Skip(request.Page * request.Size)
                    .Take(request.Size)
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