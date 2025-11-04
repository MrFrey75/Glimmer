using MongoDB.Driver;

namespace Glimmer.Core.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "GlimmerDB";
    
    // Collection names
    public string UsersCollection { get; set; } = "users";
    public string RefreshTokensCollection { get; set; } = "refreshTokens";
    public string PasswordResetTokensCollection { get; set; } = "passwordResetTokens";
    public string UniversesCollection { get; set; } = "universes";
    public string RelationsCollection { get; set; } = "relations";
}
