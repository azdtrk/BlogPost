using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs;
using MediatR;

namespace Blog.Application.CQRS.Commands.User.RefreshTokenLogin
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
