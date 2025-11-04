using Glimmer.Core.Models;
using Glimmer.Core.Enums;
using Glimmer.Core.Repositories;

namespace Glimmer.Core.Services;

public interface IEntityService
{
    // Universe Operations
    Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy);
    Task<Universe?> GetUniverseByIdAsync(Guid universeId);
    Task<List<Universe>> GetUniversesByUserAsync(Guid userId);
    Task<List<Universe>> GetAllUniversesAsync();
    Task<bool> UpdateUniverseAsync(Universe universe);
    Task<bool> DeleteUniverseAsync(Guid universeId);

    // Artifact Operations
    Task<Artifact?> CreateArtifactAsync(Guid universeId, string name, string description, ArtifactTypeEnum artifactType);
    Task<Artifact?> GetArtifactByIdAsync(Guid universeId, Guid artifactId);
    Task<List<Artifact>> GetArtifactsByUniverseAsync(Guid universeId);
    Task<bool> UpdateArtifactAsync(Guid universeId, Artifact artifact);
    Task<bool> DeleteArtifactAsync(Guid universeId, Guid artifactId);

    // CannonEvent Operations
    Task<CannonEvent?> CreateCannonEventAsync(Guid universeId, string name, string description, CannonEventTypeEnum eventType);
    Task<CannonEvent?> GetCannonEventByIdAsync(Guid universeId, Guid eventId);
    Task<List<CannonEvent>> GetCannonEventsByUniverseAsync(Guid universeId);
    Task<bool> UpdateCannonEventAsync(Guid universeId, CannonEvent cannonEvent);
    Task<bool> DeleteCannonEventAsync(Guid universeId, Guid eventId);

    // Faction Operations
    Task<Faction?> CreateFactionAsync(Guid universeId, string name, string description, FactionTypeEnum factionType);
    Task<Faction?> GetFactionByIdAsync(Guid universeId, Guid factionId);
    Task<List<Faction>> GetFactionsByUniverseAsync(Guid universeId);
    Task<bool> UpdateFactionAsync(Guid universeId, Faction faction);
    Task<bool> DeleteFactionAsync(Guid universeId, Guid factionId);

    // Location Operations
    Task<Location?> CreateLocationAsync(Guid universeId, string name, string description, LocationTypeEnum locationType, Location? parentLocation = null);
    Task<Location?> GetLocationByIdAsync(Guid universeId, Guid locationId);
    Task<List<Location>> GetLocationsByUniverseAsync(Guid universeId);
    Task<bool> UpdateLocationAsync(Guid universeId, Location location);
    Task<bool> DeleteLocationAsync(Guid universeId, Guid locationId);

    // NotableFigure Operations
    Task<NotableFigure?> CreateNotableFigureAsync(Guid universeId, string name, string description, FigureTypeEnum figureType);
    Task<NotableFigure?> GetNotableFigureByIdAsync(Guid universeId, Guid figureId);
    Task<List<NotableFigure>> GetNotableFiguresByUniverseAsync(Guid universeId);
    Task<bool> UpdateNotableFigureAsync(Guid universeId, NotableFigure figure);
    Task<bool> DeleteNotableFigureAsync(Guid universeId, Guid figureId);

    // Fact Operations
    Task<Fact?> CreateFactAsync(Guid universeId, string name, string description, string value, FactTypeEnum factType);
    Task<Fact?> GetFactByIdAsync(Guid universeId, Guid factId);
    Task<List<Fact>> GetFactsByUniverseAsync(Guid universeId);
    Task<bool> UpdateFactAsync(Guid universeId, Fact fact);
    Task<bool> DeleteFactAsync(Guid universeId, Guid factId);

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

public class EntityService : IEntityService
{
    private readonly IUniverseRepository _universeRepository;
    private readonly IRelationRepository _relationRepository;

    public EntityService(IUniverseRepository universeRepository, IRelationRepository relationRepository)
    {
        _universeRepository = universeRepository;
        _relationRepository = relationRepository;
    }

    #region Universe Operations

    public async Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy)
    {
        var universe = new Universe
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _universeRepository.CreateAsync(universe);
    }

    public async Task<Universe?> GetUniverseByIdAsync(Guid universeId)
    {
        return await _universeRepository.GetByIdAsync(universeId);
    }

    public async Task<List<Universe>> GetUniversesByUserAsync(Guid userId)
    {
        return await _universeRepository.GetByUserIdAsync(userId);
    }

    public async Task<List<Universe>> GetAllUniversesAsync()
    {
        return await _universeRepository.GetAllAsync();
    }

    public async Task<bool> UpdateUniverseAsync(Universe universe)
    {
        var existing = await _universeRepository.GetByIdAsync(universe.Uuid);
        if (existing == null) return false;

        existing.Name = universe.Name;
        existing.Description = universe.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _universeRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteUniverseAsync(Guid universeId)
    {
        var universe = await _universeRepository.GetByIdAsync(universeId);
        if (universe == null) return false;

        return await _universeRepository.DeleteAsync(universeId);
    }

    #endregion

    #region Artifact Operations

    public async Task<Artifact?> CreateArtifactAsync(Guid universeId, string name, string description, ArtifactTypeEnum artifactType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var artifact = new Artifact
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            ArtifactType = artifactType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Artifacts.Add(artifact);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return artifact;
    }

    public async Task<Artifact?> GetArtifactByIdAsync(Guid universeId, Guid artifactId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Artifacts.FirstOrDefault(a => a.Uuid == artifactId && !a.IsDeleted);
    }

    public async Task<List<Artifact>> GetArtifactsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Artifacts.Where(a => !a.IsDeleted).ToList() ?? new List<Artifact>();
    }

    public async Task<bool> UpdateArtifactAsync(Guid universeId, Artifact artifact)
    {

        var existing = await GetArtifactByIdAsync(universeId, artifact.Uuid);
        if (existing == null) return false;

        existing.Name = artifact.Name;
        existing.Description = artifact.Description;
        existing.ArtifactType = artifact.ArtifactType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteArtifactAsync(Guid universeId, Guid artifactId)
    {

        var artifact = await GetArtifactByIdAsync(universeId, artifactId);
        if (artifact == null) return false;

        artifact.IsDeleted = true;
        artifact.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }

    #endregion

    #region CannonEvent Operations

    public async Task<CannonEvent?> CreateCannonEventAsync(Guid universeId, string name, string description, CannonEventTypeEnum eventType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var cannonEvent = new CannonEvent
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            EventType = eventType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.CannonEvents.Add(cannonEvent);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return cannonEvent;
    }

    public async Task<CannonEvent?> GetCannonEventByIdAsync(Guid universeId, Guid eventId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.CannonEvents.FirstOrDefault(e => e.Uuid == eventId && !e.IsDeleted);
    }

    public async Task<List<CannonEvent>> GetCannonEventsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.CannonEvents.Where(e => !e.IsDeleted).ToList() ?? new List<CannonEvent>();
    }

    public async Task<bool> UpdateCannonEventAsync(Guid universeId, CannonEvent cannonEvent)
    {

        var existing = await GetCannonEventByIdAsync(universeId, cannonEvent.Uuid);
        if (existing == null) return false;

        existing.Name = cannonEvent.Name;
        existing.Description = cannonEvent.Description;
        existing.EventType = cannonEvent.EventType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteCannonEventAsync(Guid universeId, Guid eventId)
    {

        var cannonEvent = await GetCannonEventByIdAsync(universeId, eventId);
        if (cannonEvent == null) return false;

        cannonEvent.IsDeleted = true;
        cannonEvent.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }

    #endregion

    #region Faction Operations

    public async Task<Faction?> CreateFactionAsync(Guid universeId, string name, string description, FactionTypeEnum factionType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var faction = new Faction
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            FactionType = factionType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Factions.Add(faction);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return faction;
    }

    public async Task<Faction?> GetFactionByIdAsync(Guid universeId, Guid factionId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Factions.FirstOrDefault(f => f.Uuid == factionId && !f.IsDeleted);
    }

    public async Task<List<Faction>> GetFactionsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Factions.Where(f => !f.IsDeleted).ToList() ?? new List<Faction>();
    }

    public async Task<bool> UpdateFactionAsync(Guid universeId, Faction faction)
    {

        var existing = await GetFactionByIdAsync(universeId, faction.Uuid);
        if (existing == null) return false;

        existing.Name = faction.Name;
        existing.Description = faction.Description;
        existing.FactionType = faction.FactionType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteFactionAsync(Guid universeId, Guid factionId)
    {

        var faction = await GetFactionByIdAsync(universeId, factionId);
        if (faction == null) return false;

        faction.IsDeleted = true;
        faction.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }

    #endregion

    #region Location Operations

    public async Task<Location?> CreateLocationAsync(Guid universeId, string name, string description, LocationTypeEnum locationType, Location? parentLocation = null)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var location = new Location
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            LocationType = locationType,
            ParentLocation = parentLocation,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Locations.Add(location);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return location;
    }

    public async Task<Location?> GetLocationByIdAsync(Guid universeId, Guid locationId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Locations.FirstOrDefault(l => l.Uuid == locationId && !l.IsDeleted);
    }

    public async Task<List<Location>> GetLocationsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Locations.Where(l => !l.IsDeleted).ToList() ?? new List<Location>();
    }

    public async Task<bool> UpdateLocationAsync(Guid universeId, Location location)
    {

        var existing = await GetLocationByIdAsync(universeId, location.Uuid);
        if (existing == null) return false;

        existing.Name = location.Name;
        existing.Description = location.Description;
        existing.LocationType = location.LocationType;
        existing.ParentLocation = location.ParentLocation;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteLocationAsync(Guid universeId, Guid locationId)
    {

        var location = await GetLocationByIdAsync(universeId, locationId);
        if (location == null) return false;

        location.IsDeleted = true;
        location.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }

    #endregion

    #region NotableFigure Operations

    public async Task<NotableFigure?> CreateNotableFigureAsync(Guid universeId, string name, string description, FigureTypeEnum figureType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var figure = new NotableFigure
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            FigureType = figureType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Figures.Add(figure);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return figure;
    }

    public async Task<NotableFigure?> GetNotableFigureByIdAsync(Guid universeId, Guid figureId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Figures.FirstOrDefault(f => f.Uuid == figureId && !f.IsDeleted);
    }

    public async Task<List<NotableFigure>> GetNotableFiguresByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Figures.Where(f => !f.IsDeleted).ToList() ?? new List<NotableFigure>();
    }

    public async Task<bool> UpdateNotableFigureAsync(Guid universeId, NotableFigure figure)
    {

        var existing = await GetNotableFigureByIdAsync(universeId, figure.Uuid);
        if (existing == null) return false;

        existing.Name = figure.Name;
        existing.Description = figure.Description;
        existing.FigureType = figure.FigureType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteNotableFigureAsync(Guid universeId, Guid figureId)
    {

        var figure = await GetNotableFigureByIdAsync(universeId, figureId);
        if (figure == null) return false;

        figure.IsDeleted = true;
        figure.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }

    #endregion

    #region Fact Operations

    public async Task<Fact?> CreateFactAsync(Guid universeId, string name, string description, string value, FactTypeEnum factType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var fact = new Fact
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            Value = value,
            FactType = factType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Facts.Add(fact);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return fact;
    }

    public async Task<Fact?> GetFactByIdAsync(Guid universeId, Guid factId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Facts.FirstOrDefault(f => f.Uuid == factId && !f.IsDeleted);
    }

    public async Task<List<Fact>> GetFactsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Facts.Where(f => !f.IsDeleted).ToList() ?? new List<Fact>();
    }

    public async Task<bool> UpdateFactAsync(Guid universeId, Fact fact)
    {

        var existing = await GetFactByIdAsync(universeId, fact.Uuid);
        if (existing == null) return false;

        existing.Name = fact.Name;
        existing.Description = fact.Description;
        existing.Value = fact.Value;
        existing.FactType = fact.FactType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteFactAsync(Guid universeId, Guid factId)
    {

        var fact = await GetFactByIdAsync(universeId, factId);
        if (fact == null) return false;

        fact.IsDeleted = true;
        fact.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }

    #endregion

    #region EntityRelation Operations

    public async Task<EntityRelation?> CreateRelationAsync(Guid universeId, BaseEntity fromEntity, BaseEntity toEntity, RelationTypeEnum relationType)
    {
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var relation = new EntityRelation(universeId, fromEntity, toEntity, relationType)
        {
            Oid = await _relationRepository.GetNextIdAsync()
        };

        await _relationRepository.CreateAsync(relation);
        
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        
        return relation;
    }

    public async Task<EntityRelation?> GetRelationByIdAsync(Guid universeId, int relationId)
    {
        return await _relationRepository.GetByIdAsync(relationId);
    }

    public async Task<List<EntityRelation>> GetRelationsByUniverseAsync(Guid universeId)
    {
        return await _relationRepository.GetByUniverseIdAsync(universeId);
    }

    public async Task<List<EntityRelation>> GetRelationsByEntityAsync(Guid universeId, Guid entityId)
    {
        return await _relationRepository.GetByEntityIdAsync(universeId, entityId);
    }

    public async Task<bool> UpdateRelationAsync(Guid universeId, EntityRelation relation)
    {
        var existing = await GetRelationByIdAsync(universeId, relation.Oid);
        if (existing == null) return false;

        existing.RelationType = relation.RelationType;
        existing.UpdatedAt = DateTime.UtcNow;
        await _relationRepository.UpdateAsync(existing);

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

        return true;
    }

    public async Task<bool> DeleteRelationAsync(Guid universeId, int relationId)
    {
        var success = await _relationRepository.DeleteAsync(relationId);
        if (!success) return false;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

        return true;
    }

    #endregion

    #region Generic BaseEntity Operations

    public async Task<BaseEntity?> GetEntityByIdAsync(Guid universeId, Guid entityId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        BaseEntity? entity = universe.Artifacts.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.CannonEvents.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Factions.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Figures.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Locations.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Facts.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        return null;
    }

    public async Task<List<BaseEntity>> SearchEntitiesAsync(Guid universeId, string searchTerm)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return new List<BaseEntity>();

        var results = new List<BaseEntity>();

        results.AddRange(universe.Artifacts
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        results.AddRange(universe.CannonEvents
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        results.AddRange(universe.Factions
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        results.AddRange(universe.Figures
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        results.AddRange(universe.Locations
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        results.AddRange(universe.Facts
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        e.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        return results;
    }

    public async Task<int> GetEntityCountAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return 0;

        return universe.Artifacts.Count(e => !e.IsDeleted) +
               universe.CannonEvents.Count(e => !e.IsDeleted) +
               universe.Factions.Count(e => !e.IsDeleted) +
               universe.Figures.Count(e => !e.IsDeleted) +
               universe.Locations.Count(e => !e.IsDeleted) +
               universe.Facts.Count(e => !e.IsDeleted);
    }

    #endregion
}