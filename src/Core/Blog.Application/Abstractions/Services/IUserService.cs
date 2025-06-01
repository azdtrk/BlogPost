using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task UpdateRefreshTokenAsync(string refreshToken, ApplicationUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task<User> GetUser(Guid Id);
        Task<bool> HasRolePermissionToEndpointAsync(string userName, string code);
    }
}
