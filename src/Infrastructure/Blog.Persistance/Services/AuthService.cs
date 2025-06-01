using Blog.Application.Abstractions.Services;
using Blog.Application.CQRS.Commands.User.LoginUser;
using Blog.Application.CQRS.Commands.User.RegisterUser;
using Blog.Application.DTOs;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.User;
using Blog.Domain.Entities;
using Blog.Persistance.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        private readonly ApplicationDbContext _dbContext;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationUserRole> roleManager,
            ITokenService tokenService,
            IUserService userService,
            IUserWriteRepository userWriteRepository,
            ApplicationDbContext dbContext
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userService = userService;
            _userWriteRepository = userWriteRepository;
            _dbContext = dbContext;
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
            // Validate user doesn't already exist
            ApplicationUser existingAppUser = await _userManager.FindByEmailAsync(registerUserRequest.Email);
            if (existingAppUser != null)
                throw new UserAlreadyExistsException(registerUserRequest.Email);
            
            existingAppUser = await _userManager.FindByNameAsync(registerUserRequest.UserName);
            if (existingAppUser != null)
                throw new UserAlreadyExistsException(registerUserRequest.UserName);
            
            // Create identity user
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

            // Get created identity user and determine role
            var identityUser = await _userManager.FindByNameAsync(registerUserRequest.UserName);
            string passwordHash = identityUser?.PasswordHash ?? string.Empty;
            
            UserRole userRoleEnum = DetermineUserRole(registerUserRequest.Role);
            
            try
            {
                // Register domain user based on role
                if (userRoleEnum == UserRole.Author)
                {
                    await RegisterDomainUser<Author>(
                        identityUser, 
                        appUser, 
                        registerUserRequest, 
                        passwordHash, 
                        UserRole.Author);
                }
                else
                {
                    await RegisterDomainUser<Reader>(
                        identityUser, 
                        appUser, 
                        registerUserRequest, 
                        passwordHash, 
                        UserRole.Reader);
                }
            }
            catch (Exception e)
            {
                // Clean up on failure
                if (identityUser != null)
                    await _userManager.DeleteAsync(identityUser);
                    
                throw new ApiException($"Failed to register user: {e.Message}", e);
            }

            return await GenerateAuthenticatedUserDto(appUser);
        }

        private UserRole DetermineUserRole(string roleString)
        {
            if (!string.IsNullOrEmpty(roleString) && 
                roleString.Equals("author", StringComparison.OrdinalIgnoreCase))
            {
                return UserRole.Author;
            }
            return UserRole.Reader;
        }

        private async Task RegisterDomainUser<T>(
            ApplicationUser identityUser, 
            ApplicationUser appUser,
            RegisterUserRequest request, 
            string passwordHash,
            UserRole role) where T : User, new()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            
            try
            {
                if (identityUser != null)
                {
                    var roleResult = await _userManager.AddToRoleAsync(identityUser, role.ToString());
                    if (!roleResult.Succeeded)
                    {
                        var errors = roleResult.Errors.Select(e => e.Description);
                        throw new ApiException($"Failed to add {role} role to user: " + string.Join(", ", errors));
                    }
                }

                ApplicationUserRole? applicationRole = await GetOrCreateRole(role);
                
                T domainUser = new T
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    Name = request.UserName,
                    PasswordHash = passwordHash,
                    DateCreated = DateTime.UtcNow,
                    ApplicationUserRoleId = applicationRole.Id
                };

                await SaveDomainUser(domainUser, appUser);
                
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Transaction will be rolled back automatically
                throw;
            }
        }

        private async Task<ApplicationUserRole> GetOrCreateRole(UserRole role)
        {
            ApplicationUserRole? applicationRole = null;
            
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                applicationRole = new ApplicationUserRole
                { 
                    Id = Guid.NewGuid(), 
                    Name = role.ToString(),
                    RoleType = role
                };
                await _roleManager.CreateAsync(applicationRole);
            }
            else
            {
                applicationRole = await _roleManager.FindByNameAsync(role.ToString());
            }
            
            if (applicationRole == null)
                throw new ApiException($"Failed to find or create {role} role");
                
            return applicationRole;
        }

        private async Task SaveDomainUser<T>(T domainUser, ApplicationUser appUser) where T : User
        {
            // Add entity to appropriate DbSet based on type
            if (domainUser is Author author)
            {
                await _dbContext.Authors.AddAsync(author);
            }
            else if (domainUser is Reader reader)
            {
                await _dbContext.Readers.AddAsync(reader);
            }
            
            var saveResult = await _dbContext.SaveChangesAsync();
            if (saveResult <= 0)
            {
                throw new ApiException($"Failed to save {typeof(T).Name} to database: No rows affected");
            }
            
            domainUser.ApplicationUserId = appUser.Id;
            appUser.DomainUserId = domainUser.Id;
            
            var identityUpdateResult = await _userManager.UpdateAsync(appUser);
            if (!identityUpdateResult.Succeeded)
            {
                var errors = identityUpdateResult.Errors.Select(e => e.Description);
                throw new ApiException("Failed to update application user: " + string.Join(", ", errors));
            }
            
            if (domainUser is Author updatedAuthor)
            {
                _dbContext.Authors.Update(updatedAuthor);
            }
            else if (domainUser is Reader updatedReader)
            {
                _dbContext.Readers.Update(updatedReader);
            }
            
            var finalSaveResult = await _dbContext.SaveChangesAsync();
            if (finalSaveResult <= 0)
            {
                throw new ApiException($"Failed to update {typeof(T).Name} with application user ID");
            }
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
            if (appUser == null)
                throw new ArgumentNullException(nameof(appUser), "ApplicationUser cannot be null");
            
            TokenDto token;
            try
            {
                token = await _tokenService.GenerateTokensAsync(appUser);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Failed to generate token: {ex.Message}");
            }

            try
            {
                appUser.RefreshToken = token.RefreshToken;
                appUser.RefreshTokenEndDate = token.Expiration.AddDays(1);
                await _userManager.UpdateAsync(appUser);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Failed to update refresh token: {ex.Message}");
            }

            UserRole userRole = UserRole.Reader;
            
            try
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                if (roles.Contains(UserRole.Author.ToString()))
                {
                    userRole = UserRole.Author;
                }
            }
            catch (Exception)
            {
                // If we can't get roles, just use the default
            }

            return new AuthenticatedUserDto
            {
                Id = appUser.Id,
                UserName = appUser.UserName ?? string.Empty,
                Token = token,
                Role = userRole
            };
        }
    }
}
