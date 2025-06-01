using Blog.Application.Abstractions.Services;
using Blog.Application.Exceptions;
using Blog.Application.Helpers;
using Blog.Domain.Entities;
using Blog.Application.Repositories.User;
using Blog.Application.Repositories.Endpoint;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IEndpointReadRepository endpointReadRepository,
            IUserReadRepository userReadRepository
        )
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<User> GetUser(Guid Id)
        {
            var user = await _userReadRepository.GetByIdAsync(Id);

            if(user == null)
                throw new UserNotFoundException();

            return user;
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, ApplicationUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new UserNotFoundException();
        }

        public async Task<string[]> GetRolesToUserAsync(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                throw new UserNotFoundException();
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string userName, string code)
        {
            var userRoles = await GetRolesToUserAsync(userName);

            if (!userRoles.Any())
                return false;

            Endpoint? endpoint = await _endpointReadRepository.Table
                     .Include(e => e.Roles)
                     .FirstOrDefaultAsync(e => e.Code == code);

            if (endpoint == null)
                return false;

            var hasRole = false;
            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }

            return false;
        }
    }
}
