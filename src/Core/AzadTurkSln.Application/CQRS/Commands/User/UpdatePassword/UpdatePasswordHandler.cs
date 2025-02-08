using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.Exceptions;
using MediatR;

namespace AzadTurkSln.Application.CQRS.Commands.User.UpdatePassword
{
    public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordRequest, UpdatePasswordResponse>
    {
        readonly IUserService _userService;

        public UpdatePasswordHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordResponse> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.PasswordConfirm))
                throw new PasswordDoesNotMatchException();

            await _userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.Password);

            UpdatePasswordResponse response = new UpdatePasswordResponse
            {
                Value = $"You changed your password."
            };
            return new();
        }
    }
}
