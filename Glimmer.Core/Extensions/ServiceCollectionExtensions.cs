using Glimmer.Core.Configuration;
using Glimmer.Core.Repositories;
using Glimmer.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Glimmer.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlimmerCore(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure MongoDB serialization
        if (!BsonClassMap.IsClassMapRegistered(typeof(Glimmer.Core.Models.BaseEntity)))
        {
            // Register custom GUID serializer with legacy representation for compatibility
            var guidSerializer = new MongoDB.Bson.Serialization.Serializers.GuidSerializer(GuidRepresentation.CSharpLegacy);
            BsonSerializer.RegisterSerializer(guidSerializer);
            
            // Register conventions for MongoDB
            // Note: Not using CamelCaseElementNameConvention because we explicitly define
            // element names with [BsonElement] attributes on our models
            var conventionPack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true)
            };
            ConventionRegistry.Register("GlimmerConventions", conventionPack, t => true);
        }

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
        services.AddScoped<IDbSeedService, DbSeedService>();
        
        return services;
    }
}
