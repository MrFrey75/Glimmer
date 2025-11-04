namespace Glimmer.Core.Models;

public class Universe
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<Artifact> Artifacts { get; set; } = [];
    public List<CannonEvent> CannonEvents { get; set; } = [];
    public List<Faction> Factions { get; set; } = [];
    public List<NotableFigure> Figures { get; set; } = [];
    public List<Location> Locations { get; set; } = [];
    public List<Fact> Facts { get; set; } = [];

}