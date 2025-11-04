using MongoDB.Bson.Serialization.Attributes;

namespace Glimmer.Core.Models;

[BsonIgnoreExtraElements]
public class RefreshToken
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
    
    [BsonElement("RevokedAt")]
    public DateTime? RevokedAt { get; set; }
    
    [BsonElement("ReplacedByToken")]
    public string? ReplacedByToken { get; set; }
    
    [BsonElement("RevokedReason")]
    public string? RevokedReason { get; set; }
    
    [BsonIgnore]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    
    [BsonIgnore]
    public bool IsRevoked => RevokedAt != null;
    
    [BsonIgnore]
    public bool IsActive => !IsRevoked && !IsExpired;
}
