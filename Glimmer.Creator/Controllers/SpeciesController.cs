using Glimmer.Core.Services;
using Glimmer.Core.Models;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class SpeciesController : BaseController
{
    private readonly IEntityService _entityService;

    public SpeciesController(
        IEntityService entityService,
        ILogger<SpeciesController> logger) : base(logger)
    {
        _entityService = entityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid universeId)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(universeId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to view this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var viewModel = new SpeciesListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Species = universe.Species
                    .Where(s => !s.IsDeleted)
                    .Select(s => new SpeciesCardViewModel
                    {
                        Uuid = s.Uuid,
                        Name = s.Name,
                        Description = s.Description,
                        SpeciesType = s.SpeciesType,
                        CreatedAt = s.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading species", "Details", "Universe");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid universeId)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(universeId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to modify this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var viewModel = new CreateSpeciesViewModel
            {
                UniverseId = universeId
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading create form", "Details", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSpeciesViewModel model)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(model.UniverseId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to modify this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var species = new Species
            {
                Name = model.Name,
                Description = model.Description,
                SpeciesType = model.SpeciesType,
                AverageHeight = model.AverageHeight,
                HeightMeasure = model.HeightMeasure,
                AverageWeight = model.AverageWeight,
                WeightMeasure = model.WeightMeasure,
                LengthMeasure = model.LengthMeasure,
                AverageLength = model.AverageLength,
                SkinColor = model.SkinColor,
                EyeColor = model.EyeColor,
                HairColor = model.HairColor,
                DistinguishingFeatures = model.DistinguishingFeatures,
                NaturalHabitat = model.NaturalHabitat,
                GeographicDistribution = model.GeographicDistribution,
                SocialStructure = model.SocialStructure,
                BehavioralTraits = model.BehavioralTraits,
                Diet = model.Diet,
                AverageLifespan = model.AverageLifespan,
                ReproductiveMethods = model.ReproductiveMethods,
                GestationPeriod = model.GestationPeriod,
                OffspringPerBirth = model.OffspringPerBirth,
                CommunicationMethods = model.CommunicationMethods,
                PredatorsAndThreats = model.PredatorsAndThreats,
                ConservationStatus = model.ConservationStatus,
                HasMagicalAbilities = model.HasMagicalAbilities,
                MagicalAbilitiesDescription = model.MagicalAbilitiesDescription,
                AdditionalNotes = model.AdditionalNotes
            };

            var entity = await _entityService.CreateSpeciesAsync(model.UniverseId, species);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create species. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Species created: {SpeciesName} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Species '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating species");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid universeId, Guid id)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(universeId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to view this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var species = await _entityService.GetSpeciesByIdAsync(universeId, id);
            if (species == null)
            {
                TempData["ErrorMessage"] = "Species not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new SpeciesDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = species.Uuid,
                Name = species.Name,
                Description = species.Description,
                SpeciesType = species.SpeciesType,
                AverageHeight = species.AverageHeight,
                HeightMeasure = species.HeightMeasure,
                AverageWeight = species.AverageWeight,
                WeightMeasure = species.WeightMeasure,
                AverageLength = species.AverageLength,
                LengthMeasure = species.LengthMeasure,
                SkinColor = species.SkinColor,
                EyeColor = species.EyeColor,
                HairColor = species.HairColor,
                DistinguishingFeatures = species.DistinguishingFeatures,
                NaturalHabitat = species.NaturalHabitat,
                GeographicDistribution = species.GeographicDistribution,
                SocialStructure = species.SocialStructure,
                BehavioralTraits = species.BehavioralTraits,
                Diet = species.Diet,
                AverageLifespan = species.AverageLifespan,
                ReproductiveMethods = species.ReproductiveMethods,
                GestationPeriod = species.GestationPeriod,
                OffspringPerBirth = species.OffspringPerBirth,
                CommunicationMethods = species.CommunicationMethods,
                PredatorsAndThreats = species.PredatorsAndThreats,
                ConservationStatus = species.ConservationStatus,
                HasMagicalAbilities = species.HasMagicalAbilities,
                MagicalAbilitiesDescription = species.MagicalAbilitiesDescription,
                AdditionalNotes = species.AdditionalNotes,
                CreatedAt = species.CreatedAt,
                UpdatedAt = species.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading species details", nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid universeId, Guid id)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(universeId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to modify this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var species = await _entityService.GetSpeciesByIdAsync(universeId, id);
            if (species == null)
            {
                TempData["ErrorMessage"] = "Species not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditSpeciesViewModel
            {
                UniverseId = universeId,
                Uuid = species.Uuid,
                Name = species.Name,
                Description = species.Description,
                SpeciesType = species.SpeciesType,
                AverageHeight = species.AverageHeight,
                HeightMeasure = species.HeightMeasure,
                AverageWeight = species.AverageWeight,
                WeightMeasure = species.WeightMeasure,
                LengthMeasure = species.LengthMeasure,
                AverageLength = species.AverageLength,
                SkinColor = species.SkinColor,
                EyeColor = species.EyeColor,
                HairColor = species.HairColor,
                DistinguishingFeatures = species.DistinguishingFeatures,
                NaturalHabitat = species.NaturalHabitat,
                GeographicDistribution = species.GeographicDistribution,
                SocialStructure = species.SocialStructure,
                BehavioralTraits = species.BehavioralTraits,
                Diet = species.Diet,
                AverageLifespan = species.AverageLifespan,
                ReproductiveMethods = species.ReproductiveMethods,
                GestationPeriod = species.GestationPeriod,
                OffspringPerBirth = species.OffspringPerBirth,
                CommunicationMethods = species.CommunicationMethods,
                PredatorsAndThreats = species.PredatorsAndThreats,
                ConservationStatus = species.ConservationStatus,
                HasMagicalAbilities = species.HasMagicalAbilities,
                MagicalAbilitiesDescription = species.MagicalAbilitiesDescription,
                AdditionalNotes = species.AdditionalNotes,
                CreatedAt = species.CreatedAt,
                UpdatedAt = species.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading species for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditSpeciesViewModel model)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        if (id != model.Uuid || universeId != model.UniverseId)
        {
            TempData["ErrorMessage"] = "Invalid request.";
            return RedirectToAction(nameof(Index), new { universeId });
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(universeId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to modify this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var species = await _entityService.GetSpeciesByIdAsync(universeId, id);
            if (species == null)
            {
                TempData["ErrorMessage"] = "Species not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            species.Name = model.Name;
            species.Description = model.Description;
            species.SpeciesType = model.SpeciesType;
            species.AverageHeight = model.AverageHeight;
            species.HeightMeasure = model.HeightMeasure;
            species.AverageWeight = model.AverageWeight;
            species.WeightMeasure = model.WeightMeasure;
            species.AverageLength = model.AverageLength;
            species.LengthMeasure = model.LengthMeasure;
            species.SkinColor = model.SkinColor;
            species.EyeColor = model.EyeColor;
            species.HairColor = model.HairColor;
            species.DistinguishingFeatures = model.DistinguishingFeatures;
            species.NaturalHabitat = model.NaturalHabitat;
            species.GeographicDistribution = model.GeographicDistribution;
            species.SocialStructure = model.SocialStructure;
            species.BehavioralTraits = model.BehavioralTraits;
            species.Diet = model.Diet;
            species.AverageLifespan = model.AverageLifespan;
            species.ReproductiveMethods = model.ReproductiveMethods;
            species.GestationPeriod = model.GestationPeriod;
            species.OffspringPerBirth = model.OffspringPerBirth;
            species.CommunicationMethods = model.CommunicationMethods;
            species.PredatorsAndThreats = model.PredatorsAndThreats;
            species.ConservationStatus = model.ConservationStatus;
            species.HasMagicalAbilities = model.HasMagicalAbilities;
            species.MagicalAbilitiesDescription = model.MagicalAbilitiesDescription;
            species.AdditionalNotes = model.AdditionalNotes;
            species.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateSpeciesAsync(universeId, species);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update species. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Species updated: {SpeciesName} in universe {UniverseName}", 
                species.Name, universe.Name);
            TempData["SuccessMessage"] = $"Species '{species.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating species");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid universeId, Guid id)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(universeId);
            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction("Index", "Universe");
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to modify this universe.";
                return RedirectToAction("Index", "Universe");
            }

            var species = await _entityService.GetSpeciesByIdAsync(universeId, id);
            if (species == null)
            {
                TempData["ErrorMessage"] = "Species not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var success = await _entityService.DeleteSpeciesAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete species. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("Species deleted: {SpeciesName} in universe {UniverseName}", 
                species.Name, universe.Name);
            TempData["SuccessMessage"] = $"Species '{species.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting species", nameof(Index));
        }
    }
}
