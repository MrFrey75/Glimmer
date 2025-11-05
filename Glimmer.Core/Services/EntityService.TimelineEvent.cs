using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - TimelineEvent Operations
/// </summary>
public partial class EntityService
{
public async Task<TimelineEvent?> CreateTimelineEventAsync(Guid universeId, string name, string description, TimelineEventTypeEnum eventType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var cannonEvent = new TimelineEvent
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            EventType = eventType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.TimelineEvents.Add(cannonEvent);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return cannonEvent;
    }

    public async Task<TimelineEvent?> GetTimelineEventByIdAsync(Guid universeId, Guid eventId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.TimelineEvents.FirstOrDefault(e => e.Uuid == eventId && !e.IsDeleted);
    }

    public async Task<List<TimelineEvent>> GetTimelineEventsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.TimelineEvents.Where(e => !e.IsDeleted).ToList() ?? new List<TimelineEvent>();
    }

    public async Task<bool> UpdateTimelineEventAsync(Guid universeId, TimelineEvent cannonEvent)
    {

        var existing = await GetTimelineEventByIdAsync(universeId, cannonEvent.Uuid);
        if (existing == null) return false;

        existing.Name = cannonEvent.Name;
        existing.Description = cannonEvent.Description;
        existing.EventType = cannonEvent.EventType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteTimelineEventAsync(Guid universeId, Guid eventId)
    {

        var cannonEvent = await GetTimelineEventByIdAsync(universeId, eventId);
        if (cannonEvent == null) return false;

        cannonEvent.IsDeleted = true;
        cannonEvent.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }
}
