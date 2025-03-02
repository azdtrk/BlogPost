using Blog.Application.Abstractions.Services;
using Blog.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Blog.Application.CQRS.Commands.User.UpdatePassword
{
    public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordRequest, UpdatePasswordResponse>
    {
        private readonly IUserService _userService;

        public UpdatePasswordHandler(
            IUserService userService
        )
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordResponse> Handle(UpdatePasswordRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _userService.UpdatePasswordAsync(request.UserId.ToString(), request.ResetToken, request.Password);

                UpdatePasswordResponse response = new UpdatePasswordResponse
                {
                    Value = $"Password has been changed."
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error creating blogpost: ({ex.Message})");
            }
        }
    }
}