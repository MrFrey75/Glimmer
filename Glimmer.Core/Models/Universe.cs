using MongoDB.Bson.Serialization.Attributes;

namespace Glimmer.Core.Models;

[BsonIgnoreExtraElements]
public class Universe
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<MetadataItem> Metadata { get; set; } = [];

    // Collections of related entities

    public List<Artifact> Artifacts { get; set; } = [];
    public List<TimelineEvent> TimelineEvents { get; set; } = [];
    public List<Faction> Factions { get; set; } = [];
    public List<NotableFigure> Figures { get; set; } = [];
    public List<Location> Locations { get; set; } = [];
    public List<Fact> Facts { get; set; } = [];
    public List<Species> Species { get; set; } = [];

}

public class MetadataItem
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}