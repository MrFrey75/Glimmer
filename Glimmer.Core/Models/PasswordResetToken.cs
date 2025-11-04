using MongoDB.Bson.Serialization.Attributes;

namespace Glimmer.Core.Models;

[BsonIgnoreExtraElements]
public class PasswordResetToken
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonElement("UserId")]
    [BsonRequired]
    public Guid UserId { get; set; }
    
    [BsonElement("Token")]
    [BsonRequired]
    public string Token { get; set; } = string.Empty;
    
    [BsonElement("ExpiresAt")]
    public DateTime ExpiresAt { get; set; }
    
    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("IsUsed")]
    public bool IsUsed { get; set; } = false;
    
    [BsonElement("UsedAt")]
    public DateTime? UsedAt { get; set; }
    
    [BsonIgnore]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    
    [BsonIgnore]
    public bool IsValid => !IsUsed && !IsExpired;
}
