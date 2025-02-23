using Blog.Application.Abstractions.Services;
using Blog.Application.CQRS.Commands.User.LoginUser;
using Blog.Application.CQRS.Commands.User.RegisterUser;
using Blog.Application.DTOs;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Persistance.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationUserRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IUserWriteRepository _userWriteRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationUserRole> roleManager,
            ITokenService tokenService,
            IUserService userService,
            IUserWriteRepository userWriteRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userService = userService;
            _userWriteRepository = userWriteRepository;
        }

        public async Task<AuthenticatedUserDto> LoginAsync(LoginUserRequest loginUserRequest)
        {
            ApplicationUser appUser = await _userManager.FindByEmailAsync(loginUserRequest.UserNameOrEmail);

            if (appUser == null)
                appUser = await _userManager.FindByNameAsync(loginUserRequest.UserNameOrEmail);

            if (appUser == null)
                throw new UserNotFoundException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, loginUserRequest.Password, false);

            if (!result.Succeeded)
                throw new InvalidCredentialsException();

            return await GenerateAuthenticatedUserDto(appUser);
        }

        public async Task<AuthenticatedUserDto> RegisterAsync(RegisterUserRequest registerUserRequest)
        {
            ApplicationUser existingAppUser = await _userManager.FindByEmailAsync(registerUserRequest.Email);

            if (existingAppUser != null)
                throw new UserAlreadyExistsException(registerUserRequest.Email);

            existingAppUser = await _userManager.FindByNameAsync(registerUserRequest.UserName);

            if (existingAppUser != null)
                throw new UserAlreadyExistsException(registerUserRequest.UserName);

            ApplicationUser appUser = new ApplicationUser
            {
                Email = registerUserRequest.Email,
                UserName = registerUserRequest.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerUserRequest.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ApiException("Registration failed: " + string.Join(", ", errors));
            }

            if (!await _roleManager.RoleExistsAsync(UserRole.Reader.ToString()))
                await _roleManager.CreateAsync(new() { Id = Guid.NewGuid(), Name = UserRole.Reader.ToString() });

            await _userManager.AddToRoleAsync(appUser, UserRole.Reader.ToString());

            User domainUser = new User
            {
                ApplicationUserId = appUser.Id,
                Email = registerUserRequest.Email,
                Name = registerUserRequest.UserName,
                Password = registerUserRequest.Password
            };

            await _userWriteRepository.AddAsync(domainUser);

            return await GenerateAuthenticatedUserDto(appUser);
        }

        public async Task<AuthenticatedUserDto> RefreshTokenAsync(string refreshToken)
        {
            ApplicationUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (appUser == null)
                throw new UserNotFoundException();

            return await GenerateAuthenticatedUserDto(appUser);
        }

        public async Task<TokenDto> RefreshTokenLoginAsync(string refreshToken)
        {
            ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                TokenDto token = await _tokenService.GenerateTokensAsync(user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            else
                throw new UserNotFoundException();
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
                UserName = appUser.UserName ?? "",
                Token = token,
                Role = appUser.DomainUser.ApplicationUserRole.RoleType.ToString() == UserRole.Author.ToString() ? UserRole.Author : UserRole.Reader,
            };
        }

    }
}
