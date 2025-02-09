using AutoMapper;
using Blog.Application.DTOs.Comment;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using Blog.Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.Comment.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest, UpdateCommentResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateCommentRequest> _validator;
        private readonly ILogger<UpdateCommentHandler> _logger;

        public UpdateCommentHandler(
            ICommentWriteRepository commentWriteRepository,
            ICommentReadRepository commentReadRepository,
            IMapper mapper,
            IValidator<UpdateCommentRequest> validator,
            ILogger<UpdateCommentHandler> logger
        )
        {
            _commentWriteRepository = commentWriteRepository;
            _commentReadRepository = commentReadRepository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Update comment validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                    throw new ValidationException(validationResult.Errors);
                }

                var commentToBeUpdated = await _commentReadRepository.GetByIdAsync(request.Id);

                if (commentToBeUpdated == null)
                    throw new EntityNotFoundException(nameof(Comment), request.Id);

                commentToBeUpdated.Content = request.Content;

                _commentWriteRepository.Update(commentToBeUpdated);

                var commentDto = _mapper.Map<CommentDto>(commentToBeUpdated);

                var response = new UpdateCommentResponse()
                {
                    Value = commentDto
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error updating comment: ({ex.Message})");
            }
        }

    }
}
