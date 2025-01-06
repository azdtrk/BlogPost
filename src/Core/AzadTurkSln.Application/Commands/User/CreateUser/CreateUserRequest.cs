using MediatR;

namespace AzadTurkSln.Application.Commands.User.CreateUser
{
    public class CreateUserRequest : IRequest<CreateUserResponse>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
