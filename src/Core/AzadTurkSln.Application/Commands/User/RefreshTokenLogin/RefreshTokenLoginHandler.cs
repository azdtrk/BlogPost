using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.DTOs;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginHandler : IRequestHandler<RefreshTokenLoginRequest, RefreshTokenLoginResponse>
    {
        readonly IAuthService _authService;
        public RefreshTokenLoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<RefreshTokenLoginResponse> Handle(RefreshTokenLoginRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
