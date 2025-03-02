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

        public RegisterUserHandler(
            IAuthService authService
        )
        {
            _authService = authService;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
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