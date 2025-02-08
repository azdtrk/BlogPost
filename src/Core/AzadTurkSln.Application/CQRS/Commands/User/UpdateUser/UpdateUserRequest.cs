using AzadTurkSln.Domain.Entities;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.User.UpdateUser
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public Guid Id { get; set; }
        public string About { get; set; }
        public Image ProfilePhoto { get; set; }
    }
}
