using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - CannonEvent Operations
/// </summary>
public partial class EntityService
{
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
}
