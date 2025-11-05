using Glimmer.Core.Services;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class FactController : BaseController
{
    private readonly IEntityService _entityService;

    public FactController(
        IEntityService entityService,
        ILogger<FactController> logger) : base(logger)
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

            var viewModel = new FactListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Facts = universe.Facts
                    .Where(e => !e.IsDeleted)
                    .Select(e => new FactCardViewModel
                    {
                        Uuid = e.Uuid,
                        Name = e.Name,
                        Description = e.Description,
                        FactType = e.FactType,
                        CreatedAt = e.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading facts", "Details", "Universe");
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

            var viewModel = new CreateFactViewModel { UniverseId = universeId };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading create form", "Details", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFactViewModel model)
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

            var entity = await _entityService.CreateFactAsync(
                model.UniverseId,
                model.Name,
                model.Description,
                model.Value,
                model.FactType,
                model.AdditionalNotes);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create fact. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Fact created: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Fact '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating fact");
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

            var entity = await _entityService.GetFactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Fact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new FactDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                Value = entity.Value,
                FactType = entity.FactType,
                AdditionalNotes = entity.AdditionalNotes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading fact details", nameof(Index));
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

            var entity = await _entityService.GetFactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Fact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditFactViewModel
            {
                UniverseId = universeId,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                Value = entity.Value,
                FactType = entity.FactType,
                AdditionalNotes = entity.AdditionalNotes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading fact for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditFactViewModel model)
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

            var entity = await _entityService.GetFactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Fact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Value = model.Value;
            entity.FactType = model.FactType;
            entity.AdditionalNotes = model.AdditionalNotes;
            entity.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateFactAsync(universeId, entity);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update fact. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Fact updated: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Fact '{entity.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating fact");
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

            var entity = await _entityService.GetFactByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Fact not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var success = await _entityService.DeleteFactAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete fact. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("Fact deleted: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Fact '{entity.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting fact", nameof(Index));
        }
    }
}
