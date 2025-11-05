using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Species Operations
/// </summary>
public partial class EntityService
{
public async Task<Species?> CreateSpeciesAsync(Guid universeId, string name, string description, SpeciesTypeEnum speciesType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var species = new Species
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            SpeciesType = speciesType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Species.Add(species);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return species;
    }

    public async Task<Species?> GetSpeciesByIdAsync(Guid universeId, Guid speciesId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Species.FirstOrDefault(f => f.Uuid == speciesId && !f.IsDeleted);
    }

    public async Task<List<Species>> GetSpeciesByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Species.Where(f => !f.IsDeleted).ToList() ?? new List<Species>();
    }

    public async Task<bool> UpdateSpeciesAsync(Guid universeId, Species species)
    {

        var existing = await GetSpeciesByIdAsync(universeId, species.Uuid);
        if (existing == null) return false;

        existing.Name = species.Name;
        existing.Description = species.Description;
        existing.SpeciesType = species.SpeciesType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteSpeciesAsync(Guid universeId, Guid speciesId)
    {

        var species = await GetSpeciesByIdAsync(universeId, speciesId);
        if (species == null) return false;

        species.IsDeleted = true;
        species.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }
}
