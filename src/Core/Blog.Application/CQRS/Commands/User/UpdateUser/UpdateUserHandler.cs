using AutoMapper;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserRequest> _validator;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(
            IMapper mapper,
            IUserService userService,
            IValidator<UpdateUserRequest> validator,
            ILogger<UpdateUserHandler> logger
        )
        {
            _mapper = mapper;
            _userService = userService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Update password validation failed: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                Domain.Entities.User userToBeUpdated = _mapper.Map<Domain.Entities.User>(request);
                Domain.Entities.User updatedUser = await _userService.UpdateUserAsync(userToBeUpdated, userToBeUpdated.Id);

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
