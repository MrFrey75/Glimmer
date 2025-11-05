using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Location Operations
/// </summary>
public partial class EntityService
{
public async Task<Location?> CreateLocationAsync(Guid universeId, Location location)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        location.Uuid = Guid.NewGuid();
        location.CreatedAt = DateTime.UtcNow;
        location.UpdatedAt = DateTime.UtcNow;

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
        existing.Coordinates = location.Coordinates;
        existing.Climate = location.Climate;
        existing.Terrain = location.Terrain;
        existing.NaturalResources = location.NaturalResources;
        existing.Address = location.Address;
        existing.PoliticalAffiliation = location.PoliticalAffiliation;
        existing.Inhabitants = location.Inhabitants;
        existing.LanguagesSpoken = location.LanguagesSpoken;
        existing.HistoricalSignificance = location.HistoricalSignificance;
        existing.AdditionalNotes = location.AdditionalNotes;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

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
}
