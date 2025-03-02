using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.Comment.DeleteComment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentRequest, DeleteCommentResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly IValidator<DeleteCommentRequest> _validator;
        private readonly ILogger<DeleteCommentHandler> _logger;

        public DeleteCommentHandler(
            ICommentWriteRepository commentWriteRepository,
            ICommentReadRepository commentReadRepository,
            IValidator<DeleteCommentRequest> validator,
            ILogger<DeleteCommentHandler> logger
        )
        {
            _commentWriteRepository = commentWriteRepository;
            _commentReadRepository = commentReadRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Delete comment request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                    throw new ValidationException(validationResult.Errors);
                }

                var commentToBeDeleted = await _commentReadRepository.GetByIdAsync(request.Id);

                if (commentToBeDeleted == null)
                    throw new EntityNotFoundException(nameof(Comment), request.Id);

                _commentWriteRepository.Remove(commentToBeDeleted);

                var response = new DeleteCommentResponse()
                {
                    Value = $"Comment with Id: {request.Id} has been deleted"
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error deleting the comment: ({ex.Message})");
            }
        }
    }
}