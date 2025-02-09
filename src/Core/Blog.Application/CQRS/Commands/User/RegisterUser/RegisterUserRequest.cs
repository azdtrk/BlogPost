using MediatR;

namespace Blog.Application.CQRS.Commands.User.RegisterUser
{
    public class RegisterUserRequest : IRequest<RegisterUserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
