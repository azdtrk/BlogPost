using AutoMapper;
using AzadTurkSln.Application.Repositories;
using MediatR;

namespace AzadTurkSln.Application.Commands.User.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUserWriteRepository userWriteRepository, IMapper mapper) 
        {
            _userWriteRepository = userWriteRepository;
            _mapper = mapper;
        }

        public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Domain.Entities.User>(request);
            await _userWriteRepository.AddAsync(user);
            return new();
        }
    }
}
