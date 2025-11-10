using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Universe Operations
/// </summary>
public partial class EntityService
{
    // Dependencies are already declared in the main EntityService.cs class

    public async Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy, TimelineTypeEnum timelineType = TimelineTypeEnum.CalendarBased)
    {
        _logger.LogInformation("Creating universe '{Name}' for user {Username} (ID: {UserId}) with TimelineType: {TimelineType}",
            name, createdBy.Username, createdBy.Uuid, timelineType);

        var universe = new Universe
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedBy = createdBy,
            TimelineType = timelineType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _universeRepository.CreateAsync(universe);

        if (result != null)
        {
            _logger.LogInformation("Universe {UniverseId} created successfully by user {Username} (ID: {UserId})",
                universe.Uuid, createdBy.Username, createdBy.Uuid);

            bool needsUpdate = false;

            var defaultSpecies = await _dbSeedService.GetDefaultSpeciesAsync();
            foreach (var species in defaultSpecies)
            {
                if (result.Species == null)
                {
                    result.Species = new List<Species>();
                }
                result.Species.Add(species);
            }
            
            _logger.LogInformation("Added default species to universe {UniverseId}", universe.Uuid);

            
            needsUpdate = true;

            // Create anchor event if using relative dating
            if (timelineType == TimelineTypeEnum.RelativeDating)
            {
                _logger.LogInformation("Creating anchor event for RelativeDating universe {UniverseId}", universe.Uuid);

                var anchorEvent = new TimelineEvent
                {
                    Uuid = Guid.NewGuid(),
                    Name = "Anchor Event",
                    Description = "Primary anchor event for relative dating. This event cannot be deleted.",
                    EventType = TimelineEventTypeEnum.Other,
                    IsAnchorEvent = true,
                    UseCalendarDate = true,
                    Year = 0,
                    Era = "Year Zero",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (result.TimelineEvents == null)
                {
                    result.TimelineEvents = new List<TimelineEvent>();
                }
                result.TimelineEvents.Add(anchorEvent);
                needsUpdate = true;

                _logger.LogInformation("Anchor event {EventId} created for universe {UniverseId}", anchorEvent.Uuid, universe.Uuid);
            }

            // Consolidate updates into a single call for efficiency.
            if (needsUpdate)
            {
                await _universeRepository.UpdateAsync(result);
            }
        }
        else
        {
            _logger.LogError("Failed to create universe '{Name}' for user {Username} (ID: {UserId})",
                name, createdBy.Username, createdBy.Uuid);
        }

        return result;
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
        if (existing == null)
        {
            _logger.LogWarning("Universe update failed: Universe {UniverseId} not found", universe.Uuid);
            return false;
        }

        _logger.LogInformation("Updating universe {UniverseId} '{Name}'", universe.Uuid, universe.Name);

        existing.Name = universe.Name;
        existing.Description = universe.Description;
        // TimelineType is immutable after creation - do NOT update it
        existing.UpdatedAt = DateTime.UtcNow;

        var result = await _universeRepository.UpdateAsync(existing);

        if (result)
        {
            _logger.LogInformation("Universe {UniverseId} updated successfully", universe.Uuid);
        }

        return result;
    }

    public async Task<bool> DeleteUniverseAsync(Guid universeId)
    {
        var universe = await _universeRepository.GetByIdAsync(universeId);
        if (universe == null)
        {
            _logger.LogWarning("Universe deletion failed: Universe {UniverseId} not found", universeId);
            return false;
        }

        _logger.LogInformation("Deleting universe {UniverseId} '{Name}'", universeId, universe.Name);

        var result = await _universeRepository.DeleteAsync(universeId);

        if (result)
        {
            _logger.LogInformation("Universe {UniverseId} deleted successfully", universeId);
        }

        return result;
    }
}