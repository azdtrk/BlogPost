using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Queries.User.GetUserById
{
    public class GetUserByIdRequest : IRequest<ServiceResponse<GetUserByIdResponse>>
    {

    }
}
