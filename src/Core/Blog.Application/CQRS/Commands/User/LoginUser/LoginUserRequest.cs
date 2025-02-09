using MediatR;

namespace Blog.Application.CQRS.Commands.User.LoginUser
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
