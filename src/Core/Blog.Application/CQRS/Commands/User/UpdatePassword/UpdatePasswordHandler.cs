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
        private readonly IValidator<UpdatePasswordRequest> _validator;
        private readonly ILogger<UpdatePasswordHandler> _logger;

        public UpdatePasswordHandler(
            IUserService userService,
            IValidator<UpdatePasswordRequest> validator,
            ILogger<UpdatePasswordHandler> logger
        )
        {
            _userService = userService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UpdatePasswordResponse> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Update password validation failed: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

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
