using MediatR;

namespace Blog.Application.CQRS.Commands.User.LoginUser
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
