using Glimmer.Core.Services;
using Glimmer.Core.Repositories;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class UniverseController : BaseController
{
    private readonly IEntityService _entityService;
    private readonly IUserRepository _userRepository;

    public UniverseController(
        IEntityService entityService,
        IUserRepository userRepository,
        ILogger<UniverseController> logger) : base(logger)
    {
        _entityService = entityService;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var userId = GetCurrentUserIdAsGuid()!.Value;
            var universes = await _entityService.GetUniversesByUserAsync(userId);

            var viewModel = new UniverseListViewModel
            {
                Universes = universes.Select(u => new UniverseCardViewModel
                {
                    Uuid = u.Uuid,
                    Name = u.Name,
                    Description = u.Description,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    EntityCount = u.Figures.Count + u.Locations.Count + u.Artifacts.Count + 
                                 u.TimelineEvents.Count + u.Factions.Count + u.Facts.Count
                }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading universes", "Index", "Home");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        return View(new CreateUniverseViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUniverseViewModel model)
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
            var userId = GetCurrentUserIdAsGuid()!.Value;
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            var universe = await _entityService.CreateUniverseAsync(model.Name, model.Description, user);

            if (universe == null)
            {
                TempData["ErrorMessage"] = "Failed to create universe. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Universe created: {UniverseName} by user {Username}", universe.Name, user.Username);
            TempData["SuccessMessage"] = $"Universe '{universe.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { id = universe.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating universe");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(id);

            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to view this universe.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UniverseDetailsViewModel
            {
                Uuid = universe.Uuid,
                Name = universe.Name,
                Description = universe.Description,
                CreatedAt = universe.CreatedAt,
                UpdatedAt = universe.UpdatedAt,
                CreatedByUsername = universe.CreatedBy.Username,
                FigureCount = universe.Figures.Count,
                LocationCount = universe.Locations.Count,
                ArtifactCount = universe.Artifacts.Count,
                EventCount = universe.TimelineEvents.Count,
                FactionCount = universe.Factions.Count,
                FactCount = universe.Facts.Count,
                SpeciesCount = universe.Species.Count,
                TotalEntityCount = universe.Figures.Count + universe.Locations.Count + 
                                  universe.Artifacts.Count + universe.TimelineEvents.Count + 
                                  universe.Factions.Count + universe.Facts.Count + universe.Species.Count
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading universe details", nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(id);

            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to edit this universe.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new EditUniverseViewModel
            {
                Uuid = universe.Uuid,
                Name = universe.Name,
                Description = universe.Description,
                CreatedAt = universe.CreatedAt,
                UpdatedAt = universe.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading universe for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditUniverseViewModel model)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        if (id != model.Uuid)
        {
            TempData["ErrorMessage"] = "Invalid request.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(id);

            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to edit this universe.";
                return RedirectToAction(nameof(Index));
            }

            universe.Name = model.Name;
            universe.Description = model.Description;
            universe.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateUniverseAsync(universe);

            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update universe. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Universe updated: {UniverseName} by user {Username}", universe.Name, GetCurrentUsername());
            TempData["SuccessMessage"] = $"Universe '{universe.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { id = universe.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        try
        {
            var universe = await _entityService.GetUniverseByIdAsync(id);

            if (universe == null)
            {
                TempData["ErrorMessage"] = "Universe not found.";
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserIdAsGuid()!.Value;
            if (universe.CreatedBy.Uuid != userId)
            {
                TempData["ErrorMessage"] = "You do not have permission to delete this universe.";
                return RedirectToAction(nameof(Index));
            }

            var success = await _entityService.DeleteUniverseAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete universe. Please try again.";
                return RedirectToAction(nameof(Details), new { id });
            }

            Logger.LogInformation("Universe deleted: {UniverseName} by user {Username}", universe.Name, GetCurrentUsername());
            TempData["SuccessMessage"] = $"Universe '{universe.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting universe", nameof(Index));
        }
    }
}
