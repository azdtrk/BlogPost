using AzadTurkSln.Application.Commands.User.LoginUser;
using AzadTurkSln.Application.Commands.User.RegisterUser;
using AzadTurkSln.Application.DTOs.User;

namespace AzadTurkSln.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<AuthenticatedUserDto> LoginAsync(LoginUserRequest loginUserRequest);
        Task<AuthenticatedUserDto> RegisterAsync(RegisterUserRequest registerUserRequest);
        Task<AuthenticatedUserDto> RefreshTokenAsync(string refreshToken);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
