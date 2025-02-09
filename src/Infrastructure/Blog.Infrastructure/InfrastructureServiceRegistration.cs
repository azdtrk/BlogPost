using Blog.Application.Abstractions.Services;
using Blog.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
