using AutoMapper;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using MediatR;

namespace Blog.Application.CQRS.Commands.User.UpdateUser
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
            try
            {
                Domain.Entities.User userToBeUpdated = _mapper.Map<Domain.Entities.User>(request);
                Domain.Entities.User updatedUser =
                    await _userService.UpdateUserAsync(userToBeUpdated, userToBeUpdated.Id);

                UpdateUserResponse response = new UpdateUserResponse
                {
                    Value = _mapper.Map<UserDto>(updatedUser)
                };
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error updating the user: ({ex.Message})");
            }
        }
    }
}