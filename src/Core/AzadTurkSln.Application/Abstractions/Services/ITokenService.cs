using AzadTurkSln.Application.DTOs;
using AzadTurkSln.Domain.Entities;
using System.Security.Claims;

namespace AzadTurkSln.Application.Abstractions.Services
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokensAsync(ApplicationUser user);
        Task<ClaimsPrincipal?> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
