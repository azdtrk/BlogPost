using AutoMapper;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.User;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Queries.User.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ILogger<GetUserByIdHandler> _logger;
        private readonly IValidator<GetUserByIdRequest> _validator;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(
            IUserReadRepository userReadRepository,
            ILogger<GetUserByIdHandler> logger,
            IValidator<GetUserByIdRequest> validator,
            IMapper mapper
        )
        {
            _userReadRepository = userReadRepository;
            _logger = logger;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Update password validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                var user = _userReadRepository.GetByIdAsync(request.UserId);

                if (user == null)
                    throw new UserNotFoundException();

                var userDto = _mapper.Map<AuthorDto>(user);

                var response = new GetUserByIdResponse()
                {
                    Value = userDto
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error getting the user: ({ex.Message})");
            }
        }
    }
}