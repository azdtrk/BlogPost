using MediatR;

namespace AzadTurkSln.Application.Commands.User.UpdateUser
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public Guid Id { get; set; }
        public string About { get; set; }
    }
}
