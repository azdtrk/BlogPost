using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Queries.User.GetUserById
{
    public class GetUserByIdRequest : IRequest<ServiceResponse<GetUserByIdResponse>>
    {

    }
}
