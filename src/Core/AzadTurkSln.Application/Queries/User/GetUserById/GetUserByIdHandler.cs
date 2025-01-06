using AzadTurkSln.Application.Wrappers;
using MediatR;

namespace AzadTurkSln.Application.Queries.User.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, ServiceResponse<GetUserByIdResponse>>
    {
        public Task<ServiceResponse<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
