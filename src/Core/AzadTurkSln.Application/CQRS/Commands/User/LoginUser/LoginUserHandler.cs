using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.DTOs.User;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.User.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAuthService _authService;

        public LoginUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            AuthenticatedUserDto authenticatedUserDto = await _authService.LoginAsync(request);

            var response = new LoginUserResponse()
            {
                Value = authenticatedUserDto
            };

            return response;
        }
    }
}
