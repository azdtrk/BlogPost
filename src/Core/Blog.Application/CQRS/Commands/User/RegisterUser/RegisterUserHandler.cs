using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.User.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterUserRequest> _validator;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            IAuthService authService,
            IValidator<RegisterUserRequest> validator,
            ILogger<RegisterUserHandler> logger
        )
        {
            _authService = authService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Register user request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                AuthenticatedUserDto authenticatedUserDto = await _authService.RegisterAsync(request);

                var response = new RegisterUserResponse()
                {
                    Value = authenticatedUserDto
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error while registering the user: ({ex.Message})");
            }
        }
    }
}
