using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Universe Operations
/// </summary>
public partial class EntityService
{
public async Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy)
    {
        _logger.LogInformation("Creating universe '{Name}' for user {Username} (ID: {UserId})", name, createdBy.Username, createdBy.Uuid);
        
        var universe = new Universe
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _universeRepository.CreateAsync(universe);
        
        if (result != null)
        {
            _logger.LogInformation("Universe {UniverseId} created successfully by user {Username} (ID: {UserId})", 
                universe.Uuid, createdBy.Username, createdBy.Uuid);
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
