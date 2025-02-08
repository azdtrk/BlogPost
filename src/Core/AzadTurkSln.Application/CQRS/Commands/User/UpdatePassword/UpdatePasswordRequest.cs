using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.User.UpdatePassword
{
    public class UpdatePasswordRequest : IRequest<UpdatePasswordResponse>
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}