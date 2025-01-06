using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AzadTurkSln.Application.Mappings
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
