using AutoMapper;
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using Blog.Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.BlogPost.UpdateBlogPost
{
    public class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPostRequest, UpdateBlogPostResponse>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly IBlogPostReadRepository _blogPostReadRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateBlogPostRequest> _validator;
        private readonly ILogger<UpdateBlogPostHandler> _logger;

        public UpdateBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            IBlogPostReadRepository blogPostReadRepository,
            IMapper mapper,
            IValidator<UpdateBlogPostRequest> validator,
            ILogger<UpdateBlogPostHandler> logger)
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _blogPostReadRepository = blogPostReadRepository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UpdateBlogPostResponse> Handle(UpdateBlogPostRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Update blogpost validation failed: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                throw new ValidationException(validationResult.Errors);
            }

            var blogPostToBeUpdated = await _blogPostReadRepository.GetByIdAsync(request.Id);

            if (blogPostToBeUpdated == null)
                throw new EntityNotFoundException(nameof(BlogPost), request.Id);

            blogPostToBeUpdated.Title = request.Content;
            blogPostToBeUpdated.Preface = request.Content;
            blogPostToBeUpdated.Content = request.Content;
            blogPostToBeUpdated.ThumbnailImage = request.ThumbNailImage;
            blogPostToBeUpdated.Images = request.Images;

            _blogPostWriteRepository.Update(blogPostToBeUpdated);

            var blogPostUpdateDto = _mapper.Map<BlogPostUpdateDto>(blogPostToBeUpdated);

            var response = new UpdateBlogPostResponse()
            {
                Value = blogPostUpdateDto
            };

            return response;

        }
    }
}
