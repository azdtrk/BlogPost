using MediatR;

namespace Blog.Application.CQRS.Commands.User.UpdatePassword
{
    public class UpdatePasswordRequest : IRequest<UpdatePasswordResponse>
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string ResetToken { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}