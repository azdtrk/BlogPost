using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Blog.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
