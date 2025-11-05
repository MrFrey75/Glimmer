using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Generic BaseEntity Operations
/// </summary>
public partial class EntityService
{
public async Task<BaseEntity?> GetEntityByIdAsync(Guid universeId, Guid entityId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        BaseEntity? entity = universe.Artifacts.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.TimelineEvents.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Factions.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Figures.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Locations.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Facts.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (entity != null) return entity;

        entity = universe.Species.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
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

        results.AddRange(universe.TimelineEvents
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

        results.AddRange(universe.Species
            .Where(e => !e.IsDeleted && 
                       (e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                        e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))));

        return results;
    }

    public async Task<int> GetEntityCountAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return 0;

        return universe.Artifacts.Count(e => !e.IsDeleted) +
               universe.TimelineEvents.Count(e => !e.IsDeleted) +
               universe.Factions.Count(e => !e.IsDeleted) +
               universe.Figures.Count(e => !e.IsDeleted) +
               universe.Locations.Count(e => !e.IsDeleted) +
               universe.Facts.Count(e => !e.IsDeleted) + 
               universe.Species.Count(e => !e.IsDeleted);
    }
}
