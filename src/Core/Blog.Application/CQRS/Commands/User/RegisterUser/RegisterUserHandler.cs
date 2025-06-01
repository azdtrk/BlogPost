using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.User.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(
            IAuthService authService,
            ILogger<RegisterUserHandler> logger
        )
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AuthenticatedUserDto authenticatedUserDto = await _authService.RegisterAsync(request);
                
                if (authenticatedUserDto == null)
                {
                    _logger.LogError("AuthService.RegisterAsync returned null");
                    throw new ApiException("Registration failed: Authentication service returned null");
                }
                
                if (authenticatedUserDto.Token == null)
                {
                    _logger.LogError("Token is null in authenticatedUserDto");
                    throw new ApiException("Registration failed: Token generation issue");
                }
                
                _logger.LogInformation("Successfully registered user {Username} with ID {UserId}", 
                    request.UserName, authenticatedUserDto.Id);

                var response = new RegisterUserResponse()
                {
                    Value = authenticatedUserDto
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Username}: {Message}", 
                    request.UserName, ex.Message);
                
                // Create a service response with error details
                var errorResponse = new RegisterUserResponse
                {
                    IsSuccess = false,
                    Message = $"Error during registration: {ex.Message}",
                    Value = null
                };
                
                return errorResponse;  // Return error response instead of throwing
            }
        }
    }
}