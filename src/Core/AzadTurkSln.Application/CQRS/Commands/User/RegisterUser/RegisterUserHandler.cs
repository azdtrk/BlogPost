using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.DTOs.User;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.User.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IAuthService _authService;

        public RegisterUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            AuthenticatedUserDto authenticatedUserDto = await _authService.RegisterAsync(request);

            var response = new RegisterUserResponse()
            {
                Value = authenticatedUserDto
            };

            return response;
        }
    }
}
