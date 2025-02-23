using AutoMapper;
using Blog.Application.DTOs.Comment;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost
{
    public class GetCommentByBlogPostHandler : IRequestHandler<GetCommentByBlogPostRequest, GetCommentByBlogPostResponse>
    {
        private readonly IBlogPostReadRepository _blogPostReadRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCommentByBlogPostRequest> _validator;
        private readonly ILogger<GetCommentByBlogPostHandler> _logger;

        public GetCommentByBlogPostHandler(
            IBlogPostReadRepository blogPostReadRepository,
            IMapper mapper,
            IValidator<GetCommentByBlogPostRequest> validator,
            ILogger<GetCommentByBlogPostHandler> logger
        )
        {
            _blogPostReadRepository = blogPostReadRepository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetCommentByBlogPostResponse> Handle(GetCommentByBlogPostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Get all comments request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                var blogPost = await _blogPostReadRepository.GetSingleAsync(x => x.Id == request.BlogPostId);

                if (blogPost == null)
                    throw new EntityNotFoundException(nameof(BlogPost), request.BlogPostId);

                var response = new GetCommentByBlogPostResponse()
                {
                    Value = _mapper.Map<List<CommentDto>>(blogPost.Comments)
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"An error occured while getting comments: ({ex.Message})");
            }
        }
    }
}
