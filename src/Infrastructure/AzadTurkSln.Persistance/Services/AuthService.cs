using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.Commands.User.LoginUser;
using AzadTurkSln.Application.Commands.User.RegisterUser;
using AzadTurkSln.Application.DTOs;
using AzadTurkSln.Application.DTOs.User;
using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AzadTurkSln.Persistance.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthenticatedUserDto> LoginAsync(LoginUserRequest loginUserRequest)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(loginUserRequest.Email);

            if (appUser == null)
                throw new UserNotFoundException(loginUserRequest.Email);

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, loginUserRequest.Password, false);

            if (!result.Succeeded)
                throw new InvalidCredentialsException();

            return await GenerateAuthenticatedUserDto(appUser);
        }

        public async Task<AuthenticatedUserDto> RegisterAsync(RegisterUserRequest registerUserRequest)
        {
            ApplicationUser existingAppUser = await _userManager.FindByEmailAsync(registerUserRequest.Email);

            if (existingAppUser != null)
                throw new UserAlreadyExistsException();

            ApplicationUser appUser = new ApplicationUser
            {
                Email = registerUserRequest.Email,
                UserName = registerUserRequest.Name
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerUserRequest.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ApiException("Registration failed: " + string.Join(", ", errors));
            }

            if (!await _roleManager.RoleExistsAsync("Reader"))
            {
                var readerRole = new IdentityRole("Reader");
                await _roleManager.CreateAsync(readerRole);
            }

            await _userManager.AddToRoleAsync(appUser, UserRole.Reader.ToString());

            return await GenerateAuthenticatedUserDto(appUser);
        }

        public async Task<AuthenticatedUserDto> RefreshTokenAsync(string refreshToken)
        {
            ApplicationUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (appUser == null)
                throw new UserNotFoundException();

            return await GenerateAuthenticatedUserDto(appUser);
        }

        public async Task<AuthenticatedUserDto> GenerateAuthenticatedUserDto(ApplicationUser appUser)
        {
            TokenDto token = await _tokenService.GenerateTokensAsync(appUser);

            appUser.RefreshToken = token.RefreshToken;
            appUser.RefreshTokenEndDate = token.Expiration.AddDays(1);

            await _userManager.UpdateAsync(appUser);

            return new AuthenticatedUserDto()
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Token = token,
                Role = UserRole.Reader
            };
        }

    }
}
