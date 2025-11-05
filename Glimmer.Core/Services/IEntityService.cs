using Glimmer.Core.Models;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// Service interface for managing all entity types within universes.
/// Implementation is split across partial classes for maintainability.
/// </summary>
public interface IEntityService
{
    // Universe Operations
    Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy, TimelineTypeEnum timelineType = TimelineTypeEnum.CalendarBased);
    Task<Universe?> GetUniverseByIdAsync(Guid universeId);
    Task<List<Universe>> GetUniversesByUserAsync(Guid userId);
    Task<List<Universe>> GetAllUniversesAsync();
    Task<bool> UpdateUniverseAsync(Universe universe);
    Task<bool> DeleteUniverseAsync(Guid universeId);

    // Artifact Operations
    Task<Artifact?> CreateArtifactAsync(Guid universeId, Artifact artifact);
    Task<Artifact?> GetArtifactByIdAsync(Guid universeId, Guid artifactId);
    Task<List<Artifact>> GetArtifactsByUniverseAsync(Guid universeId);
    Task<bool> UpdateArtifactAsync(Guid universeId, Artifact artifact);
    Task<bool> DeleteArtifactAsync(Guid universeId, Guid artifactId);

    // TimelineEvent Operations
    Task<TimelineEvent?> CreateTimelineEventAsync(Guid universeId, string name, string description, TimelineEventTypeEnum eventType);
    Task<TimelineEvent?> GetTimelineEventByIdAsync(Guid universeId, Guid eventId);
    Task<List<TimelineEvent>> GetTimelineEventsByUniverseAsync(Guid universeId);
    Task<bool> UpdateTimelineEventAsync(Guid universeId, TimelineEvent timelineEvent);
    Task<bool> DeleteTimelineEventAsync(Guid universeId, Guid eventId);

    // Faction Operations
    Task<Faction?> CreateFactionAsync(Guid universeId, Faction faction);
    Task<Faction?> GetFactionByIdAsync(Guid universeId, Guid factionId);
    Task<List<Faction>> GetFactionsByUniverseAsync(Guid universeId);
    Task<bool> UpdateFactionAsync(Guid universeId, Faction faction);
    Task<bool> DeleteFactionAsync(Guid universeId, Guid factionId);

    // Location Operations
    Task<Location?> CreateLocationAsync(Guid universeId, Location location);
    Task<Location?> GetLocationByIdAsync(Guid universeId, Guid locationId);
    Task<List<Location>> GetLocationsByUniverseAsync(Guid universeId);
    Task<bool> UpdateLocationAsync(Guid universeId, Location location);
    Task<bool> DeleteLocationAsync(Guid universeId, Guid locationId);

    // NotableFigure Operations
    Task<NotableFigure?> CreateNotableFigureAsync(Guid universeId, NotableFigure figure);
    Task<NotableFigure?> GetNotableFigureByIdAsync(Guid universeId, Guid figureId);
    Task<List<NotableFigure>> GetNotableFiguresByUniverseAsync(Guid universeId);
    Task<bool> UpdateNotableFigureAsync(Guid universeId, NotableFigure figure);
    Task<bool> DeleteNotableFigureAsync(Guid universeId, Guid figureId);

    // Fact Operations
    Task<Fact?> CreateFactAsync(Guid universeId, string name, string description, string value, FactTypeEnum factType, string additionalNotes);
    Task<Fact?> GetFactByIdAsync(Guid universeId, Guid factId);
    Task<List<Fact>> GetFactsByUniverseAsync(Guid universeId);
    Task<bool> UpdateFactAsync(Guid universeId, Fact fact);
    Task<bool> DeleteFactAsync(Guid universeId, Guid factId);

    // Species Operations
    Task<Species?> CreateSpeciesAsync(Guid universeId, Species species);
    Task<Species?> GetSpeciesByIdAsync(Guid universeId, Guid speciesId);
    Task<List<Species>> GetSpeciesByUniverseAsync(Guid universeId);
    Task<bool> UpdateSpeciesAsync(Guid universeId, Species species);
    Task<bool> DeleteSpeciesAsync(Guid universeId, Guid speciesId);

    // EntityRelation Operations
    Task<EntityRelation?> CreateRelationAsync(Guid universeId, BaseEntity fromEntity, BaseEntity toEntity, RelationTypeEnum relationType);
    Task<EntityRelation?> GetRelationByIdAsync(Guid universeId, int relationId);
    Task<List<EntityRelation>> GetRelationsByUniverseAsync(Guid universeId);
    Task<List<EntityRelation>> GetRelationsByEntityAsync(Guid universeId, Guid entityId);
    Task<bool> UpdateRelationAsync(Guid universeId, EntityRelation relation);
    Task<bool> DeleteRelationAsync(Guid universeId, int relationId);

    // Generic BaseEntity Operations
    Task<BaseEntity?> GetEntityByIdAsync(Guid universeId, Guid entityId);
    Task<List<BaseEntity>> SearchEntitiesAsync(Guid universeId, string searchTerm);
    Task<int> GetEntityCountAsync(Guid universeId);
}
