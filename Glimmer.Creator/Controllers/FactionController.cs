using Glimmer.Core.Services;
using Glimmer.Core.Models;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class FactionController : BaseController
{
    private readonly IEntityService _entityService;

    public FactionController(
        IEntityService entityService,
        ILogger<FactionController> logger) : base(logger)
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

            var viewModel = new FactionListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Factions = universe.Factions
                    .Where(e => !e.IsDeleted)
                    .Select(e => new FactionCardViewModel
                    {
                        Uuid = e.Uuid,
                        Name = e.Name,
                        Description = e.Description,
                        FactionType = e.FactionType,
                        CreatedAt = e.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading factions", "Details", "Universe");
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

            var viewModel = new CreateFactionViewModel { UniverseId = universeId };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading create form", "Details", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFactionViewModel model)
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

            var faction = new Faction
            {
                Name = model.Name,
                Description = model.Description,
                FactionType = model.FactionType,
                LeadershipStructure = model.LeadershipStructure,
                MembershipCriteria = model.MembershipCriteria,
                Hierarchy = model.Hierarchy,
                PrimaryGoals = model.PrimaryGoals,
                Motivations = model.Motivations,
                KeyActivities = model.KeyActivities,
                FoundingHistory = model.FoundingHistory,
                EvolutionOverTime = model.EvolutionOverTime,
                AdditionalNotes = model.AdditionalNotes
            };

            var entity = await _entityService.CreateFactionAsync(model.UniverseId, faction);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create faction. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Faction created: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Faction '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating faction");
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

            var entity = await _entityService.GetFactionByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Faction not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new FactionDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                FactionType = entity.FactionType,
                LeadershipStructure = entity.LeadershipStructure,
                MembershipCriteria = entity.MembershipCriteria,
                Hierarchy = entity.Hierarchy,
                PrimaryGoals = entity.PrimaryGoals,
                Motivations = entity.Motivations,
                KeyActivities = entity.KeyActivities,
                FoundingHistory = entity.FoundingHistory,
                EvolutionOverTime = entity.EvolutionOverTime,
                AdditionalNotes = entity.AdditionalNotes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading faction details", nameof(Index));
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

            var entity = await _entityService.GetFactionByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Faction not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditFactionViewModel
            {
                UniverseId = universeId,
                Uuid = entity.Uuid,
                Name = entity.Name,
                Description = entity.Description,
                FactionType = entity.FactionType,
                LeadershipStructure = entity.LeadershipStructure,
                MembershipCriteria = entity.MembershipCriteria,
                Hierarchy = entity.Hierarchy,
                PrimaryGoals = entity.PrimaryGoals,
                Motivations = entity.Motivations,
                KeyActivities = entity.KeyActivities,
                FoundingHistory = entity.FoundingHistory,
                EvolutionOverTime = entity.EvolutionOverTime,
                AdditionalNotes = entity.AdditionalNotes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading faction for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditFactionViewModel model)
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

            var entity = await _entityService.GetFactionByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Faction not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.FactionType = model.FactionType;
            entity.LeadershipStructure = model.LeadershipStructure;
            entity.MembershipCriteria = model.MembershipCriteria;
            entity.Hierarchy = model.Hierarchy;
            entity.PrimaryGoals = model.PrimaryGoals;
            entity.Motivations = model.Motivations;
            entity.KeyActivities = model.KeyActivities;
            entity.FoundingHistory = model.FoundingHistory;
            entity.EvolutionOverTime = model.EvolutionOverTime;
            entity.AdditionalNotes = model.AdditionalNotes;
            entity.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateFactionAsync(universeId, entity);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update faction. Please try again.";
                return View(model);
            }

            Logger.LogInformation("Faction updated: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Faction '{entity.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating faction");
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

            var entity = await _entityService.GetFactionByIdAsync(universeId, id);
            if (entity == null)
            {
                TempData["ErrorMessage"] = "Faction not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var success = await _entityService.DeleteFactionAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete faction. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("Faction deleted: {Name} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Faction '{entity.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting faction", nameof(Index));
        }
    }
}
