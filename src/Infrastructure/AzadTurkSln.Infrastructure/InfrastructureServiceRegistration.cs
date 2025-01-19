using AzadTurkSln.Application.Abstractions.Services;
using AzadTurkSln.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace AzadTurkSln.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
