using Glimmer.Core.Configuration;
using Glimmer.Core.Repositories;
using Glimmer.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Glimmer.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlimmerCore(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure MongoDB
        var mongoSettings = configuration.GetSection("MongoDB").Get<MongoDbSettings>() ?? new MongoDbSettings();
        
        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(mongoSettings.ConnectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoSettings.DatabaseName);
        });

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUniverseRepository, UniverseRepository>();
        services.AddScoped<IRelationRepository, RelationRepository>();

        // Register services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEntityService, EntityService>();
        
        return services;
    }
}
