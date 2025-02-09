using Blog.Application.DTOs.User;
using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<User> UpdateUserAsync(User updateUserRequest, Guid Id);
        Task<User> GetUser(Guid Id);
    }
}
