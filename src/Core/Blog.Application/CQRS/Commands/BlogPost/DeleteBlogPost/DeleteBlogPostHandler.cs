using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostRequest, DeleteBlogPostResponse>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly IBlogPostReadRepository _blogPostReadRepository;
        private readonly ILogger<DeleteBlogPostHandler> _logger;
        private readonly IValidator<DeleteBlogPostRequest> _validator;

        public DeleteBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            IBlogPostReadRepository blogPostReadRepository,
            ILogger<DeleteBlogPostHandler> logger,
            IValidator<DeleteBlogPostRequest> validator)
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _blogPostReadRepository = blogPostReadRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<DeleteBlogPostResponse> Handle(DeleteBlogPostRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Delete blogpost request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                throw new ValidationException(validationResult.Errors);
            }

            var blogPostToBeDeleted = await _blogPostReadRepository.GetByIdAsync(request.Id);

            if (blogPostToBeDeleted == null)
                throw new EntityNotFoundException(nameof(BlogPost), request.Id);

            _blogPostWriteRepository.Remove(blogPostToBeDeleted);

            var response = new DeleteBlogPostResponse()
            {
                Value = $"BlogPost with Id: {request.Id} has been deleted"
            };

            return response;
        }
    }
}
