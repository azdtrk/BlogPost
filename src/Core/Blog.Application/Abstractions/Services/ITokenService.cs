using Blog.Application.DTOs;
using Blog.Domain.Entities;
using System.Security.Claims;

namespace Blog.Application.Abstractions.Services
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokensAsync(ApplicationUser user);
        Task<ClaimsPrincipal?> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
