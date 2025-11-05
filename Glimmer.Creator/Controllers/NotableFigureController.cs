using Glimmer.Core.Services;
using Glimmer.Core.Models;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Glimmer.Creator.Controllers;

public class NotableFigureController : BaseController
{
    private readonly IEntityService _entityService;

    public NotableFigureController(
        IEntityService entityService,
        ILogger<NotableFigureController> logger) : base(logger)
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

            var viewModel = new NotableFigureListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Figures = universe.Figures
                    .Where(f => !f.IsDeleted)
                    .Select(f => new NotableFigureCardViewModel
                    {
                        Uuid = f.Uuid,
                        Name = f.Name,
                        Description = f.Description,
                        FigureType = f.FigureType,
                        CreatedAt = f.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading figures", "Details", "Universe");
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

            var viewModel = new CreateNotableFigureViewModel
            {
                UniverseId = universeId
            };

            // Load species list for dropdown
            var species = await _entityService.GetSpeciesByUniverseAsync(universeId);
            ViewBag.SpeciesList = new SelectList(species, "Uuid", "Name");

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading create form", "Details", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateNotableFigureViewModel model)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        if (!ModelState.IsValid)
        {
            // Reload species list
            var speciesList = await _entityService.GetSpeciesByUniverseAsync(model.UniverseId);
            ViewBag.SpeciesList = new SelectList(speciesList, "Uuid", "Name");
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

            // Get species if selected
            Species? species = null;
            if (model.SpeciesId.HasValue)
            {
                species = universe.Species.FirstOrDefault(s => s.Uuid == model.SpeciesId.Value);
            }

            var figure = new NotableFigure
            {
                Name = model.Name,
                Description = model.Description,
                FigureType = model.FigureType,
                Species = species,
                Height = model.Height ?? string.Empty,
                HeightMeasure = model.HeightMeasure,
                Weight = model.Weight ?? string.Empty,
                WeightMeasure = model.WeightMeasure,
                EyeColor = model.EyeColor ?? string.Empty,
                HairColor = model.HairColor ?? string.Empty,
                SkinColor = model.SkinColor ?? string.Empty,
                DistinguishingFeatures = model.DistinguishingFeatures ?? string.Empty,
                Gender = model.Gender,
                SexualOrientation = model.SexualOrientation,
                HasMagicalAbilities = model.HasMagicalAbilities,
                MagicalAbilitiesDescription = model.MagicalAbilitiesDescription ?? string.Empty,
                BirthDate = model.BirthDate,
                DeathDate = model.DeathDate,
                Occupation = model.Occupation ?? string.Empty,
                NotableAchievements = model.NotableAchievements ?? string.Empty,
                Biography = model.Biography ?? string.Empty,
                AdditionalNotes = model.AdditionalNotes ?? string.Empty
            };

            var entity = await _entityService.CreateNotableFigureAsync(model.UniverseId, figure);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create figure. Please try again.";
                // Reload species list
                var speciesListReload = await _entityService.GetSpeciesByUniverseAsync(model.UniverseId);
                ViewBag.SpeciesList = new SelectList(speciesListReload, "Uuid", "Name");
                return View(model);
            }

            Logger.LogInformation("NotableFigure created: {FigureName} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Figure '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating figure");
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

            var figure = await _entityService.GetNotableFigureByIdAsync(universeId, id);
            if (figure == null)
            {
                TempData["ErrorMessage"] = "Figure not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new NotableFigureDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = figure.Uuid,
                Name = figure.Name,
                Description = figure.Description,
                FigureType = figure.FigureType,
                Species = figure.Species,
                Height = figure.Height,
                HeightMeasure = figure.HeightMeasure,
                Weight = figure.Weight,
                WeightMeasure = figure.WeightMeasure,
                EyeColor = figure.EyeColor,
                HairColor = figure.HairColor,
                SkinColor = figure.SkinColor,
                DistinguishingFeatures = figure.DistinguishingFeatures,
                Gender = figure.Gender,
                SexualOrientation = figure.SexualOrientation,
                HasMagicalAbilities = figure.HasMagicalAbilities,
                MagicalAbilitiesDescription = figure.MagicalAbilitiesDescription,
                BirthDate = figure.BirthDate,
                DeathDate = figure.DeathDate,
                Occupation = figure.Occupation,
                NotableAchievements = figure.NotableAchievements,
                Biography = figure.Biography,
                AdditionalNotes = figure.AdditionalNotes,
                CreatedAt = figure.CreatedAt,
                UpdatedAt = figure.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading figure details", nameof(Index));
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

            var figure = await _entityService.GetNotableFigureByIdAsync(universeId, id);
            if (figure == null)
            {
                TempData["ErrorMessage"] = "Figure not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditNotableFigureViewModel
            {
                UniverseId = universeId,
                Uuid = figure.Uuid,
                Name = figure.Name,
                Description = figure.Description,
                FigureType = figure.FigureType,
                SpeciesId = figure.Species?.Uuid,
                Height = figure.Height,
                HeightMeasure = figure.HeightMeasure,
                Weight = figure.Weight,
                WeightMeasure = figure.WeightMeasure,
                EyeColor = figure.EyeColor,
                HairColor = figure.HairColor,
                SkinColor = figure.SkinColor,
                DistinguishingFeatures = figure.DistinguishingFeatures,
                Gender = figure.Gender,
                SexualOrientation = figure.SexualOrientation,
                HasMagicalAbilities = figure.HasMagicalAbilities,
                MagicalAbilitiesDescription = figure.MagicalAbilitiesDescription,
                BirthDate = figure.BirthDate,
                DeathDate = figure.DeathDate,
                Occupation = figure.Occupation,
                NotableAchievements = figure.NotableAchievements,
                Biography = figure.Biography,
                AdditionalNotes = figure.AdditionalNotes,
                CreatedAt = figure.CreatedAt,
                UpdatedAt = figure.UpdatedAt
            };

            // Load species list for dropdown
            var species = await _entityService.GetSpeciesByUniverseAsync(universeId);
            ViewBag.SpeciesList = new SelectList(species, "Uuid", "Name");

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading figure for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditNotableFigureViewModel model)
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

            var figure = await _entityService.GetNotableFigureByIdAsync(universeId, id);
            if (figure == null)
            {
                TempData["ErrorMessage"] = "Figure not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            // Get species if selected
            Species? species = null;
            if (model.SpeciesId.HasValue)
            {
                species = universe.Species.FirstOrDefault(s => s.Uuid == model.SpeciesId.Value);
            }

            figure.Name = model.Name;
            figure.Description = model.Description;
            figure.FigureType = model.FigureType;
            figure.Species = species;
            figure.Height = model.Height ?? string.Empty;
            figure.HeightMeasure = model.HeightMeasure;
            figure.Weight = model.Weight ?? string.Empty;
            figure.WeightMeasure = model.WeightMeasure;
            figure.EyeColor = model.EyeColor ?? string.Empty;
            figure.HairColor = model.HairColor ?? string.Empty;
            figure.SkinColor = model.SkinColor ?? string.Empty;
            figure.DistinguishingFeatures = model.DistinguishingFeatures ?? string.Empty;
            figure.Gender = model.Gender;
            figure.SexualOrientation = model.SexualOrientation;
            figure.HasMagicalAbilities = model.HasMagicalAbilities;
            figure.MagicalAbilitiesDescription = model.MagicalAbilitiesDescription ?? string.Empty;
            figure.BirthDate = model.BirthDate;
            figure.DeathDate = model.DeathDate;
            figure.Occupation = model.Occupation ?? string.Empty;
            figure.NotableAchievements = model.NotableAchievements ?? string.Empty;
            figure.Biography = model.Biography ?? string.Empty;
            figure.AdditionalNotes = model.AdditionalNotes ?? string.Empty;
            figure.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateNotableFigureAsync(universeId, figure);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update figure. Please try again.";
                // Reload species list
                var speciesList = await _entityService.GetSpeciesByUniverseAsync(universeId);
                ViewBag.SpeciesList = new SelectList(speciesList, "Uuid", "Name");
                return View(model);
            }

            Logger.LogInformation("NotableFigure updated: {FigureName} in universe {UniverseName}", 
                figure.Name, universe.Name);
            TempData["SuccessMessage"] = $"Figure '{figure.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating figure");
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

            var figure = await _entityService.GetNotableFigureByIdAsync(universeId, id);
            if (figure == null)
            {
                TempData["ErrorMessage"] = "Figure not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var success = await _entityService.DeleteNotableFigureAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete figure. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("NotableFigure deleted: {FigureName} in universe {UniverseName}", 
                figure.Name, universe.Name);
            TempData["SuccessMessage"] = $"Figure '{figure.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting figure", nameof(Index));
        }
    }
}
