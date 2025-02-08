using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace AzadTurkSln.Application.CQRS.Commands.Comment.DeleteComment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentRequest, DeleteCommentResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IValidator<DeleteCommentRequest> _validator;
        private readonly ILogger<DeleteCommentHandler> _logger;

        public DeleteCommentHandler(
            ICommentWriteRepository commentWriteRepository,
            IValidator<DeleteCommentRequest> validator,
            ILogger<DeleteCommentHandler> logger
        )
        {
            _commentWriteRepository = commentWriteRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
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

                await _commentWriteRepository.RemoveAsync(request.Id);

                var response = new DeleteCommentResponse()
                {
                    Message = $"Comment with Id: {request.Id} has been deleted"
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
