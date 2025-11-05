using Glimmer.Core.Services;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class CannonEventController : BaseController
{
    private readonly IEntityService _entityService;

    public CannonEventController(
        IEntityService entityService,
        ILogger<CannonEventController> logger) : base(logger)
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

            var viewModel = new CannonEventListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Events = universe.CannonEvents
                    .Where(e => !e.IsDeleted)
                    .Select(e => new CannonEventCardViewModel
                    {
                        Uuid = e.Uuid,
                        Name = e.Name,
                        Description = e.Description,
                        EventType = e.EventType,
                        CreatedAt = e.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading events", "Details", "Universe");
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

            var viewModel = new CreateCannonEventViewModel { UniverseId = universeId };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading create form", "Details", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCannonEventViewModel model)
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

            var entity = await _entityService.CreateCannonEventAsync(
                model.UniverseId,
                model.Name,
                model.Description,
                model.EventType);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create event. Please try again.";
                return View(model);
            }

            Logger.LogInformation("CannonEvent created: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"CannonEvent '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating event");
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

            var entity = await _entityService.GetCannonEventByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "CannonEvent not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new CannonEventDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                EventType = entity.EventType,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading event details", nameof(Index));
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

            var entity = await _entityService.GetCannonEventByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "CannonEvent not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditCannonEventViewModel
            {
                UniverseId = universeId,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                EventType = entity.EventType,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading event for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditCannonEventViewModel model)
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

            var entity = await _entityService.GetCannonEventByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "CannonEvent not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.EventType = model.EventType;
            entity.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateCannonEventAsync(universeId, entity);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update event. Please try again.";
                return View(model);
            }

            Logger.LogInformation("CannonEvent updated: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"CannonEvent '{entity.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating event");
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

            var entity = await _entityService.GetCannonEventByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "CannonEvent not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var success = await _entityService.DeleteCannonEventAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete event. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("CannonEvent deleted: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"CannonEvent '{entity.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting event", nameof(Index));
        }
    }
}
