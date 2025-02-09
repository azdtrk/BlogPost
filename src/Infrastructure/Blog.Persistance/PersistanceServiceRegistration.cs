using Blog.Application.Abstractions.Services;
using Blog.Application.Repositories;
using Blog.Domain.Entities;
using Blog.Persistance.Context;
using Blog.Persistance.Repositories;
using Blog.Persistance.Services;
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

            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<ICommentReadRepository, CommentReadRepository>();
            services.AddScoped<ICommentWriteRepository, CommentWriteRepository>();

            services.AddScoped<IBlogPostReadRepository, BlogPostReadRepository>();
            services.AddScoped<IBlogPostWriteRepository, BlogPostWriteRepository>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}