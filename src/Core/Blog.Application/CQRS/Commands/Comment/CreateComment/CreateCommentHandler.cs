using AutoMapper;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.Comment.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentRequest, CreateCommentResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCommentRequest> _validator;
        private readonly ILogger<CreateCommentHandler> _logger;

        public CreateCommentHandler(
            ICommentWriteRepository commentWriteRepository,
            IMapper mapper,
            IValidator<CreateCommentRequest> validator,
            ILogger<CreateCommentHandler> logger)
        {
            _commentWriteRepository = commentWriteRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<CreateCommentResponse> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Comment create validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                    throw new ValidationException(validationResult.Errors);
                }

                var comment = _mapper.Map<Domain.Entities.Comment>(request);

                // ToDo: Implement a better approach of handling the mapping errors
                if (comment == null)
                    throw new MappingException();

                await _commentWriteRepository.AddAsync(comment);

                var response = new CreateCommentResponse
                {
                    Value = "Comment has been received.",
                    CreatedDate = comment.DateCreated
                };
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error creating comment: ({ex.Message})");
            }
        }
    }
}
