using Glimmer.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Glimmer.Core.Models;

[BsonIgnoreExtraElements]
public class EntityRelation
{
    [BsonId]
    public int Oid { get; set; } = 0;
    public Guid UniverseId { get; set; }
    public BaseEntity FromEntity { get; set; }
    public BaseEntity ToEntity { get; set; }
    public RelationTypeEnum RelationType { get; set; } = RelationTypeEnum.Unknown;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public string Description => $"{FromEntity?.Name ?? "Unknown"} - {RelationType} -> {ToEntity?.Name ?? "Unknown"}";

    public EntityRelation()
    {
        FromEntity = null!;
        ToEntity = null!;
    }

    public EntityRelation(Guid universeId, BaseEntity fromEntity, BaseEntity toEntity, RelationTypeEnum relationType)
    {
        UniverseId = universeId;
        FromEntity = fromEntity;
        ToEntity = toEntity;
        RelationType = relationType;
    }

}
