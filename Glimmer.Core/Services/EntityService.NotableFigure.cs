using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - NotableFigure Operations
/// </summary>
public partial class EntityService
{
public async Task<NotableFigure?> CreateNotableFigureAsync(Guid universeId, NotableFigure figure)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        figure.Uuid = Guid.NewGuid();
        figure.CreatedAt = DateTime.UtcNow;
        figure.UpdatedAt = DateTime.UtcNow;

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
        existing.Species = figure.Species;
        existing.Height = figure.Height;
        existing.Weight = figure.Weight;
        existing.HeightMeasure = figure.HeightMeasure;
        existing.WeightMeasure = figure.WeightMeasure;
        existing.EyeColor = figure.EyeColor;
        existing.HairColor = figure.HairColor;
        existing.SkinColor = figure.SkinColor;
        existing.DistinguishingFeatures = figure.DistinguishingFeatures;
        existing.Gender = figure.Gender;
        existing.SexualOrientation = figure.SexualOrientation;
        existing.HasMagicalAbilities = figure.HasMagicalAbilities;
        existing.MagicalAbilitiesDescription = figure.MagicalAbilitiesDescription;
        existing.BirthDate = figure.BirthDate;
        existing.DeathDate = figure.DeathDate;
        existing.Occupation = figure.Occupation;
        existing.NotableAchievements = figure.NotableAchievements;
        existing.Biography = figure.Biography;
        existing.AdditionalNotes = figure.AdditionalNotes;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

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
}
