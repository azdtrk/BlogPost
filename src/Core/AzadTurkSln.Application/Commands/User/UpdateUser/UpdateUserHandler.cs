using AutoMapper;
using AzadTurkSln.Application.Repositories;
using MediatR;

namespace AzadTurkSln.Application.Commands.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var userToBeUpdated = _userReadRepository.GetByIdAsync(request.Id);
            var user = _mapper.Map<Domain.Entities.User>(userToBeUpdated);
            _userWriteRepository.Update(user);
            return new();
        }
    }
}
