using Glimmer.Core.Services;
using Glimmer.Core.Models;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class LocationController : BaseController
{
    private readonly IEntityService _entityService;

    public LocationController(
        IEntityService entityService,
        ILogger<LocationController> logger) : base(logger)
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

            var viewModel = new LocationListViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Locations = universe.Locations
                    .Where(l => !l.IsDeleted)
                    .Select(l => new LocationCardViewModel
                    {
                        Uuid = l.Uuid,
                        Name = l.Name,
                        Description = l.Description,
                        LocationType = l.LocationType,
                        ParentLocationName = l.ParentLocation?.Name,
                        CreatedAt = l.CreatedAt
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading locations", "Details", "Universe");
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

            var viewModel = new CreateLocationViewModel
            {
                UniverseId = universeId,
                AvailableParentLocations = universe.Locations
                    .Where(l => !l.IsDeleted)
                    .Select(l => new LocationSelectItem
                    {
                        Id = l.Uuid,
                        Name = l.Name,
                        Type = l.LocationType
                    }).ToList()
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
    public async Task<IActionResult> Create(CreateLocationViewModel model)
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }

        if (!ModelState.IsValid)
        {
            var universe = await _entityService.GetUniverseByIdAsync(model.UniverseId);
            if (universe != null)
            {
                model.AvailableParentLocations = universe.Locations
                    .Where(l => !l.IsDeleted)
                    .Select(l => new LocationSelectItem
                    {
                        Id = l.Uuid,
                        Name = l.Name,
                        Type = l.LocationType
                    }).ToList();
            }
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

            // Get parent location if specified
            var parentLocation = model.ParentLocationId.HasValue
                ? await _entityService.GetLocationByIdAsync(model.UniverseId, model.ParentLocationId.Value)
                : null;

            var location = new Location
            {
                Name = model.Name,
                Description = model.Description,
                LocationType = model.LocationType,
                ParentLocation = parentLocation,
                Coordinates = model.Coordinates,
                Climate = model.Climate,
                Terrain = model.Terrain,
                NaturalResources = model.NaturalResources,
                Address = model.Address,
                PoliticalAffiliation = model.PoliticalAffiliation,
                Inhabitants = model.Inhabitants,
                LanguagesSpoken = model.LanguagesSpoken,
                HistoricalSignificance = model.HistoricalSignificance,
                AdditionalNotes = model.AdditionalNotes
            };

            var entity = await _entityService.CreateLocationAsync(model.UniverseId, location);

            if (entity == null)
            {
                TempData["ErrorMessage"] = "Failed to create location. Please try again.";
                model.AvailableParentLocations = universe.Locations
                    .Where(l => !l.IsDeleted)
                    .Select(l => new LocationSelectItem
                    {
                        Id = l.Uuid,
                        Name = l.Name,
                        Type = l.LocationType
                    }).ToList();
                return View(model);
            }

            Logger.LogInformation("Location created: {LocationName} in universe {UniverseName}", 
                entity.Name, universe.Name);
            TempData["SuccessMessage"] = $"Location '{entity.Name}' created successfully!";
            return RedirectToAction(nameof(Details), new { universeId = model.UniverseId, id = entity.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating location");
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

            var location = await _entityService.GetLocationByIdAsync(universeId, id);
            if (location == null)
            {
                TempData["ErrorMessage"] = "Location not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            // Get child locations
            var childLocations = universe.Locations
                .Where(l => !l.IsDeleted && l.ParentLocation?.Uuid == id)
                .Select(l => new LocationCardViewModel
                {
                    Uuid = l.Uuid,
                    Name = l.Name,
                    Description = l.Description,
                    LocationType = l.LocationType,
                    CreatedAt = l.CreatedAt
                }).ToList();

            var viewModel = new LocationDetailsViewModel
            {
                UniverseId = universeId,
                UniverseName = universe.Name,
                Uuid = location.Uuid,
                Name = location.Name,
                Description = location.Description,
                LocationType = location.LocationType,
                ParentLocationName = location.ParentLocation?.Name,
                ParentLocationId = location.ParentLocation?.Uuid,
                ChildLocations = childLocations,
                Coordinates = location.Coordinates,
                Climate = location.Climate,
                Terrain = location.Terrain,
                NaturalResources = location.NaturalResources,
                Address = location.Address,
                PoliticalAffiliation = location.PoliticalAffiliation,
                Inhabitants = location.Inhabitants,
                LanguagesSpoken = location.LanguagesSpoken,
                HistoricalSignificance = location.HistoricalSignificance,
                AdditionalNotes = location.AdditionalNotes,
                CreatedAt = location.CreatedAt,
                UpdatedAt = location.UpdatedAt
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading location details", nameof(Index));
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

            var location = await _entityService.GetLocationByIdAsync(universeId, id);
            if (location == null)
            {
                TempData["ErrorMessage"] = "Location not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            var viewModel = new EditLocationViewModel
            {
                UniverseId = universeId,
                Uuid = location.Uuid,
                Name = location.Name,
                Description = location.Description,
                LocationType = location.LocationType,
                ParentLocationId = location.ParentLocation?.Uuid,
                Coordinates = location.Coordinates,
                Climate = location.Climate,
                Terrain = location.Terrain,
                NaturalResources = location.NaturalResources,
                Address = location.Address,
                PoliticalAffiliation = location.PoliticalAffiliation,
                Inhabitants = location.Inhabitants,
                LanguagesSpoken = location.LanguagesSpoken,
                HistoricalSignificance = location.HistoricalSignificance,
                AdditionalNotes = location.AdditionalNotes,
                CreatedAt = location.CreatedAt,
                UpdatedAt = location.UpdatedAt,
                AvailableParentLocations = universe.Locations
                    .Where(l => !l.IsDeleted && l.Uuid != id) // Exclude self
                    .Select(l => new LocationSelectItem
                    {
                        Id = l.Uuid,
                        Name = l.Name,
                        Type = l.LocationType
                    }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading location for editing", nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid universeId, Guid id, EditLocationViewModel model)
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
            var universe = await _entityService.GetUniverseByIdAsync(model.UniverseId);
            if (universe != null)
            {
                model.AvailableParentLocations = universe.Locations
                    .Where(l => !l.IsDeleted && l.Uuid != id)
                    .Select(l => new LocationSelectItem
                    {
                        Id = l.Uuid,
                        Name = l.Name,
                        Type = l.LocationType
                    }).ToList();
            }
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

            var location = await _entityService.GetLocationByIdAsync(universeId, id);
            if (location == null)
            {
                TempData["ErrorMessage"] = "Location not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            // Get parent location if specified
            var parentLocation = model.ParentLocationId.HasValue
                ? await _entityService.GetLocationByIdAsync(model.UniverseId, model.ParentLocationId.Value)
                : null;

            location.Name = model.Name;
            location.Description = model.Description;
            location.LocationType = model.LocationType;
            location.ParentLocation = parentLocation;
            location.Coordinates = model.Coordinates;
            location.Climate = model.Climate;
            location.Terrain = model.Terrain;
            location.NaturalResources = model.NaturalResources;
            location.Address = model.Address;
            location.PoliticalAffiliation = model.PoliticalAffiliation;
            location.Inhabitants = model.Inhabitants;
            location.LanguagesSpoken = model.LanguagesSpoken;
            location.HistoricalSignificance = model.HistoricalSignificance;
            location.AdditionalNotes = model.AdditionalNotes;
            location.UpdatedAt = DateTime.UtcNow;

            var success = await _entityService.UpdateLocationAsync(universeId, location);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update location. Please try again.";
                model.AvailableParentLocations = universe.Locations
                    .Where(l => !l.IsDeleted && l.Uuid != id)
                    .Select(l => new LocationSelectItem
                    {
                        Id = l.Uuid,
                        Name = l.Name,
                        Type = l.LocationType
                    }).ToList();
                return View(model);
            }

            Logger.LogInformation("Location updated: {LocationName} in universe {UniverseName}", 
                location.Name, universe.Name);
            TempData["SuccessMessage"] = $"Location '{location.Name}' updated successfully!";
            return RedirectToAction(nameof(Details), new { universeId, id });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating location");
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

            var location = await _entityService.GetLocationByIdAsync(universeId, id);
            if (location == null)
            {
                TempData["ErrorMessage"] = "Location not found.";
                return RedirectToAction(nameof(Index), new { universeId });
            }

            // Check if location has children
            var hasChildren = universe.Locations.Any(l => !l.IsDeleted && l.ParentLocation?.Uuid == id);
            if (hasChildren)
            {
                TempData["ErrorMessage"] = "Cannot delete location with child locations. Please delete or move child locations first.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            var success = await _entityService.DeleteLocationAsync(universeId, id);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete location. Please try again.";
                return RedirectToAction(nameof(Details), new { universeId, id });
            }

            Logger.LogInformation("Location deleted: {LocationName} in universe {UniverseName}", 
                location.Name, universe.Name);
            TempData["SuccessMessage"] = $"Location '{location.Name}' deleted successfully.";
            return RedirectToAction(nameof(Index), new { universeId });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting location", nameof(Index));
        }
    }
}
