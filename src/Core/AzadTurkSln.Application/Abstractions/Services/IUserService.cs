using AzadTurkSln.Application.DTOs.User;
using AzadTurkSln.Domain.Entities;

namespace AzadTurkSln.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<User> UpdateUserAsync(User updateUserRequest, Guid Id);
        Task<User> GetUser(Guid Id);
    }
}
