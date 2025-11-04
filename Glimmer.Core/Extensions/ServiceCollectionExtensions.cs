using Glimmer.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Glimmer.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlimmerCore(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEntityService, EntityService>();
        
        return services;
    }
}
