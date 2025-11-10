using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Species Operations
/// </summary>
public partial class EntityService
{
public async Task<Species?> CreateSpeciesAsync(Guid universeId, Species species)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        species.Uuid = Guid.NewGuid();
        species.CreatedAt = DateTime.UtcNow;
        species.UpdatedAt = DateTime.UtcNow;

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
        existing.AverageHeight = species.AverageHeight;
        existing.AverageWeight = species.AverageWeight;
        existing.HeightMeasure = species.HeightMeasure;
        existing.WeightMeasure = species.WeightMeasure;
        existing.AverageLength = species.AverageLength;
        existing.LengthMeasure = species.LengthMeasure;
        existing.SkinColor = species.SkinColor;
        existing.EyeColor = species.EyeColor;
        existing.HairColor = species.HairColor;
        existing.DistinguishingFeatures = species.DistinguishingFeatures;
        existing.NaturalHabitat = species.NaturalHabitat;
        existing.GeographicDistribution = species.GeographicDistribution;
        existing.SocialStructure = species.SocialStructure;
        existing.BehavioralTraits = species.BehavioralTraits;
        existing.Diet = species.Diet;
        existing.AverageLifespan = species.AverageLifespan;
        existing.ReproductiveMethods = species.ReproductiveMethods;
        existing.GestationPeriod = species.GestationPeriod;
        existing.OffspringPerBirth = species.OffspringPerBirth;
        existing.CommunicationMethods = species.CommunicationMethods;
        existing.PredatorsAndThreats = species.PredatorsAndThreats;
        existing.ConservationStatus = species.ConservationStatus;
        existing.HasMagicalAbilities = species.HasMagicalAbilities;
        existing.MagicalAbilitiesDescription = species.MagicalAbilitiesDescription;
        existing.AdditionalNotes = species.AdditionalNotes;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

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
