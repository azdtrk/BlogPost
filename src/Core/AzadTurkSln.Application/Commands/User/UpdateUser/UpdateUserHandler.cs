using AutoMapper;
using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.DTOs.User;
using MediatR;

namespace AzadTurkSln.Application.Commands.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UpdateUserHandler(
            IMapper mapper,
            IUserService userService
        )
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.User userToBeUpdated = _mapper.Map<Domain.Entities.User>(request);
            Domain.Entities.User updatedUser = await _userService.UpdateUserAsync(userToBeUpdated, userToBeUpdated.Id);

            UpdateUserResponse response = new UpdateUserResponse
            {
                Value = _mapper.Map<UpdateUserDto>(updatedUser)
            };
            return response;
        }
    }
}
