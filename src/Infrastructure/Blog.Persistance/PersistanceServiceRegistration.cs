using Blog.Application.Abstractions.Services;
using Blog.Domain.Entities;
using Blog.Persistance.Context;
using Blog.Persistance.Services;
using Blog.Application.Repositories.User;
using Blog.Application.Repositories.Comment;
using Blog.Application.Repositories.BlogPost;
using Blog.Application.Repositories.Endpoint;
using Blog.Application.Repositories.Image;
using Blog.Application.Repositories.Author;
using Blog.Persistance.Repositories.BlogPost;
using Blog.Persistance.Repositories.Comment;
using Blog.Persistance.Repositories.Endpoint;
using Blog.Persistance.Repositories.User;
using Blog.Persistance.Repositories.Image;
using Blog.Persistance.Repositories.Author;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Persistance
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));

            #region Repositories
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<ICommentReadRepository, CommentReadRepository>();
            services.AddScoped<ICommentWriteRepository, CommentWriteRepository>();

            services.AddScoped<IBlogPostReadRepository, BlogPostReadRepository>();
            services.AddScoped<IBlogPostWriteRepository, BlogPostWriteRepository>();

            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();

            services.AddScoped<IImageReadRepository, ImageReadRepository>();
            services.AddScoped<IImageWriteRepository, ImageWriteRepository>();

            services.AddScoped<IAuthorReadRepository, AuthorReadRepository>();
            services.AddScoped<IAuthorWriteRepository, AuthorWriteRepository>();
            #endregion

            #region Services
            services.AddScoped<IAuthService, AuthService>(sp =>
            {
                var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                var signInManager = sp.GetRequiredService<SignInManager<ApplicationUser>>();
                var roleManager = sp.GetRequiredService<RoleManager<ApplicationUserRole>>();
                var tokenService = sp.GetRequiredService<ITokenService>();
                var userService = sp.GetRequiredService<IUserService>();
                var userWriteRepository = sp.GetRequiredService<IUserWriteRepository>();
                var dbContext = sp.GetRequiredService<ApplicationDbContext>();
                
                return new AuthService(
                    userManager,
                    signInManager,
                    roleManager,
                    tokenService,
                    userService,
                    userWriteRepository,
                    dbContext
                );
            });
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorService, AuthorService>();
            #endregion

            #region Identity Server Configurations
            services.AddIdentity<ApplicationUser, ApplicationUserRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);

            })
            .AddRoles<ApplicationUserRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            #endregion

            return services;
        }
    }
}