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

        var timelineEvent = new TimelineEvent
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            EventType = eventType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.TimelineEvents.Add(timelineEvent);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return timelineEvent;
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

    public async Task<bool> UpdateTimelineEventAsync(Guid universeId, TimelineEvent timelineEvent)
    {

        var existing = await GetTimelineEventByIdAsync(universeId, timelineEvent.Uuid);
        if (existing == null) return false;

        existing.Name = timelineEvent.Name;
        existing.Description = timelineEvent.Description;
        existing.EventType = timelineEvent.EventType;
        existing.UseCalendarDate = timelineEvent.UseCalendarDate;
        existing.Year = timelineEvent.Year;
        existing.Month = timelineEvent.Month;
        existing.Day = timelineEvent.Day;
        existing.Era = timelineEvent.Era;
        existing.AnchorEventId = timelineEvent.AnchorEventId;
        existing.YearsFromAnchor = timelineEvent.YearsFromAnchor;
        existing.RelativeDescription = timelineEvent.RelativeDescription;
        existing.DurationInDays = timelineEvent.DurationInDays;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteTimelineEventAsync(Guid universeId, Guid eventId)
    {

        var timelineEvent = await GetTimelineEventByIdAsync(universeId, eventId);
        if (timelineEvent == null) return false;

        timelineEvent.IsDeleted = true;
        timelineEvent.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }
}
