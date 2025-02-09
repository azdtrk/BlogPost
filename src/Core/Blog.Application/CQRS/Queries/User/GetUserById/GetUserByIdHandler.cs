using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Queries.User.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, ServiceResponse<GetUserByIdResponse>>
    {
        public Task<ServiceResponse<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
