using Glimmer.Core.Services;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

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

            var figure = await _entityService.CreateNotableFigureAsync(
                model.UniverseId,
                model.Name,
                model.Description,
                model.FigureType);

            if (figure == null)
            {
                TempData["ErrorMessage"] = "Failed to create figure. Please try again.";
                return View(model);
            }

            Logger.LogInformation("NotableFigure created: {FigureName} in universe {UniverseName}", 
                figure.Name, universe.Name);
            TempData["SuccessMessage"] = $"Figure '{figure.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = figure.Uuid });
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
                CreatedAt = figure.CreatedAt,
                UpdatedAt = figure.UpdatedAt
            };

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

            figure.Name = model.Name;
            figure.Description = model.Description;
            figure.FigureType = model.FigureType;
            figure.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateNotableFigureAsync(universeId, figure);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update figure. Please try again.";
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
