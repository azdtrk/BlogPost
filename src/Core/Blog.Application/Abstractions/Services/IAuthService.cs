using Blog.Application.CQRS.Commands.User.LoginUser;
using Blog.Application.CQRS.Commands.User.RegisterUser;
using Blog.Application.DTOs.User;

namespace Blog.Application.Abstractions.Services
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
