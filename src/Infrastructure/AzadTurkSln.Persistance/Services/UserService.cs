using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.DTOs.User;
using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Application.Helpers;
using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Domain.Entities;
using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace AzadTurkSln.Persistance.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IEndpointReadRepository endpointReadRepository,
            IUserWriteRepository userWriteRepository,
            IUserReadRepository userReadRepository
        )
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
            _userWriteRepository = userWriteRepository;
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

        public async Task<User> UpdateUserAsync(User updateUserRequest, Guid Id)
        {
            var userToBeUpdated = await _userReadRepository.GetByIdAsync(Id);

            if(userToBeUpdated == null)
                throw new UserNotFoundException("User you want to update couldn't be found");

            userToBeUpdated.About = updateUserRequest.About;
            userToBeUpdated.ProfilePhoto = updateUserRequest.ProfilePhoto;
            userToBeUpdated.Name = updateUserRequest.Name;

            return userToBeUpdated;
        }
    }
}
