using Glimmer.Core.Services;
using Glimmer.Core.Models;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class ArtifactController : BaseController
{
    private readonly IEntityService _entityService;

    public ArtifactController(
        IEntityService entityService,
        ILogger<ArtifactController> logger) : base(logger)
    {
        _entityService = entityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid universeId)
    {
        if (!IsAuthenticated()) return RedirectToLogin();

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

            var viewModel = new ArtifactListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Artifacts = universe.Artifacts
                    .Where(e => !e.IsDeleted)
                    .Select(e => new ArtifactCardViewModel
                    {
                        Uuid = e.Uuid,
                        Name = e.Name,
                        Description = e.Description,
                        ArtifactType = e.ArtifactType,
                        CreatedAt = e.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading artifacts", "Details", "Universe");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid universeId)
    {
        if (!IsAuthenticated()) return RedirectToLogin();

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

            var viewModel = new CreateArtifactViewModel { UniverseId = universeId };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading create form", "Details", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateArtifactViewModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();
        if (!ModelState.IsValid) return View(model);

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

            var artifact = new Artifact
            {
                Name = model.Name,
                Description = model.Description,
                ArtifactType = model.ArtifactType,
                MaterialComposition = model.MaterialComposition,
                Dimensions = model.Dimensions,
                Weight = model.Weight,
                Color = model.Color,
                Condition = model.Condition,
                Origin = model.Origin,
                HistoricalPeriod = model.HistoricalPeriod,
                CulturalSignificance = model.CulturalSignificance,
                NotableOwners = model.NotableOwners,
                HasMagicalProperties = model.HasMagicalProperties,
                MagicalPropertiesDescription = model.MagicalPropertiesDescription,
                AdditionalNotes = model.AdditionalNotes
            };

            var entity = await _entityService.CreateArtifactAsync(model.UniverseId, artifact);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create artifact. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Artifact created: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Artifact '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating artifact");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid universeId, Guid id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();

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

            var entity = await _entityService.GetArtifactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Artifact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new ArtifactDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                ArtifactType = entity.ArtifactType,
                MaterialComposition = entity.MaterialComposition,
                Dimensions = entity.Dimensions,
                Weight = entity.Weight,
                Color = entity.Color,
                Condition = entity.Condition,
                Origin = entity.Origin,
                HistoricalPeriod = entity.HistoricalPeriod,
                CulturalSignificance = entity.CulturalSignificance,
                NotableOwners = entity.NotableOwners,
                HasMagicalProperties = entity.HasMagicalProperties,
                MagicalPropertiesDescription = entity.MagicalPropertiesDescription,
                AdditionalNotes = entity.AdditionalNotes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading artifact details", nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid universeId, Guid id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();

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

            var entity = await _entityService.GetArtifactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Artifact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditArtifactViewModel
            {
                UniverseId = universeId,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                ArtifactType = entity.ArtifactType,
                MaterialComposition = entity.MaterialComposition,
                Dimensions = entity.Dimensions,
                Weight = entity.Weight,
                Color = entity.Color,
                Condition = entity.Condition,
                Origin = entity.Origin,
                HistoricalPeriod = entity.HistoricalPeriod,
                CulturalSignificance = entity.CulturalSignificance,
                NotableOwners = entity.NotableOwners,
                HasMagicalProperties = entity.HasMagicalProperties,
                MagicalPropertiesDescription = entity.MagicalPropertiesDescription,
                AdditionalNotes = entity.AdditionalNotes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading artifact for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditArtifactViewModel model)
    {
        if (!IsAuthenticated()) return RedirectToLogin();

        if (id != model.Uuid || universeId != model.UniverseId)
        {
            TempData["ErrorMessage"] = "Invalid request.";
            return RedirectToAction(nameof(Index), new { universeId });
        }

        if (!ModelState.IsValid) return View(model);

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

            var entity = await _entityService.GetArtifactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Artifact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.ArtifactType = model.ArtifactType;
            entity.MaterialComposition = model.MaterialComposition;
            entity.Dimensions = model.Dimensions;
            entity.Weight = model.Weight;
            entity.Color = model.Color;
            entity.Condition = model.Condition;
            entity.Origin = model.Origin;
            entity.HistoricalPeriod = model.HistoricalPeriod;
            entity.CulturalSignificance = model.CulturalSignificance;
            entity.NotableOwners = model.NotableOwners;
            entity.HasMagicalProperties = model.HasMagicalProperties;
            entity.MagicalPropertiesDescription = model.MagicalPropertiesDescription;
            entity.AdditionalNotes = model.AdditionalNotes;
            entity.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateArtifactAsync(universeId, entity);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update artifact. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Artifact updated: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Artifact '{entity.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating artifact");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid universeId, Guid id)
    {
        if (!IsAuthenticated()) return RedirectToLogin();

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

            var entity = await _entityService.GetArtifactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Artifact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var success = await _entityService.DeleteArtifactAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete artifact. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("Artifact deleted: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Artifact '{entity.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting artifact", nameof(Index));
        }
    }
}
