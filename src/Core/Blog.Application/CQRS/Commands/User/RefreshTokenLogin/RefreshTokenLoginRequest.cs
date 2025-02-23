using MediatR;

namespace Blog.Application.CQRS.Commands.User.RefreshTokenLogin
{
    public class RefreshTokenLoginRequest : IRequest<RefreshTokenLoginResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
