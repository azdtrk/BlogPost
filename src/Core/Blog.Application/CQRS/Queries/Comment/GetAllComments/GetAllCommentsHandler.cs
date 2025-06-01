using AutoMapper;
using Blog.Application.DTOs.Comment;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.Comment;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Queries.Comment.GetAllComments
{
    public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsRequest, GetAllCommentsResponse>
    {
        private readonly ICommentReadRepository _commentReadRpository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAllCommentsRequest> _validator;
        private readonly ILogger<GetAllCommentsHandler> _logger;

        public GetAllCommentsHandler(
            ICommentReadRepository commentReadRpository,
            IMapper mapper,
            IValidator<GetAllCommentsRequest> validator,
            ILogger<GetAllCommentsHandler> logger
        )
        {
            _commentReadRpository = commentReadRpository;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetAllCommentsResponse> Handle(GetAllCommentsRequest request,
            CancellationToken cancellationToken)
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

                var comments = _commentReadRpository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                    .Include(c => c.BlogPost)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var commentListDto = _mapper.Map<List<CommentDto>>(comments);

                if (commentListDto == null)
                    throw new MappingException();

                var response = new GetAllCommentsResponse()
                {
                    Value = commentListDto
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