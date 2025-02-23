using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.User.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginUserRequest> _validator;
        private readonly ILogger<LoginUserHandler> _logger;

        public LoginUserHandler(
            IAuthService authService,
            IValidator<LoginUserRequest> validator,
            ILogger<LoginUserHandler> logger
        )
        {
            _authService = authService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {

            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Login user request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                AuthenticatedUserDto authenticatedUserDto = await _authService.LoginAsync(request);

                var response = new LoginUserResponse()
                {
                    Value = authenticatedUserDto
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error during logging the user in: ({ex.Message})");
            }
        }
    }
}
