using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Application.Commands.User.LoginUser;
using AzadTurkSln.Application.Commands.User.RegisterUser;
using AzadTurkSln.Application.DTOs;
using AzadTurkSln.Application.DTOs.User;
using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Domain.Entities;
using ETicaretAPI.Application.Repositories;
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
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IEndpointReadRepository endpointReadRepository,
            IUserWriteRepository userWriteRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _endpointReadRepository = endpointReadRepository;
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

            User domainUser = new User
            {
                ApplicationUserId = appUser.Id,
                Email = registerUserRequest.Email,
                Name = registerUserRequest.Name,
                Password = registerUserRequest.Password
            };

            await _userWriteRepository.AddAsync(domainUser);

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

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesToUserAsync(name);

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

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return Array.Empty<string>();
        }
    }
}
