using Glimmer.Core.Enums;
using Glimmer.Core.Models;
using Glimmer.Core.Services;
using Glimmer.Creator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Glimmer.Creator.Controllers;

public class RelationController : BaseController
{
    private readonly IEntityService _entityService;

    public RelationController(IEntityService entityService, ILogger<RelationController> logger) : base(logger)
    {
        _entityService = entityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid id)
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
                return RedirectToAction("Index", "Universe");
            }

            var relations = await _entityService.GetRelationsByUniverseAsync(id);

            var viewModel = new EntityRelationListViewModel
            {
                UniverseId = universe.Uuid,
                UniverseName = universe.Name,
                Relations = relations.Select(r => new EntityRelationCardViewModel
                {
                    RelationId = r.Oid,
                    FromEntityName = r.FromEntity?.Name ?? "Unknown",
                    FromEntityType = GetEntityTypeName(r.FromEntity),
                    ToEntityName = r.ToEntity?.Name ?? "Unknown",
                    ToEntityType = GetEntityTypeName(r.ToEntity),
                    RelationType = r.RelationType,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading relationships", "Index", "Universe");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
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
                return RedirectToAction("Index", "Universe");
            }

            var viewModel = new EntityRelationViewModel
            {
                UniverseId = universe.Uuid,
                UniverseName = universe.Name
            };

            ViewBag.Entities = GetAllEntitiesForPicker(universe);
            ViewBag.RelationTypes = Enum.GetValues<RelationTypeEnum>().Select(e => new { Value = (int)e, Text = e.ToString() });

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading relationship creation form", "Index", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid id, EntityRelationViewModel model)
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
                return RedirectToAction("Index", "Universe");
            }

            if (!model.FromEntityId.HasValue || !model.ToEntityId.HasValue || !model.RelationType.HasValue)
            {
                ModelState.AddModelError("", "Please select both entities and a relationship type.");
                ViewBag.Entities = GetAllEntitiesForPicker(universe);
                ViewBag.RelationTypes = Enum.GetValues<RelationTypeEnum>().Select(e => new { Value = (int)e, Text = e.ToString() });
                return View(model);
            }

            var (fromEntity, fromType) = FindEntityById(universe, model.FromEntityId.Value);
            var (toEntity, toType) = FindEntityById(universe, model.ToEntityId.Value);

            if (fromEntity == null || toEntity == null)
            {
                ModelState.AddModelError("", "One or both selected entities could not be found.");
                ViewBag.Entities = GetAllEntitiesForPicker(universe);
                ViewBag.RelationTypes = Enum.GetValues<RelationTypeEnum>().Select(e => new { Value = (int)e, Text = e.ToString() });
                return View(model);
            }

            await _entityService.CreateRelationAsync(universe.Uuid, fromEntity, toEntity, model.RelationType.Value);

            TempData["SuccessMessage"] = $"Relationship '{fromEntity.Name} {model.RelationType} {toEntity.Name}' created successfully.";
            return RedirectToAction(nameof(Index), new { id = universe.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "creating relationship", "Index", "Universe");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, int relationId)
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
                return RedirectToAction("Index", "Universe");
            }

            var relation = await _entityService.GetRelationByIdAsync(id, relationId);

            if (relation == null || relation.UniverseId != id)
            {
                TempData["ErrorMessage"] = "Relationship not found.";
                return RedirectToAction(nameof(Index), new { id });
            }

            var viewModel = new EntityRelationViewModel
            {
                UniverseId = universe.Uuid,
                UniverseName = universe.Name,
                RelationId = relation.Oid,
                FromEntityId = relation.FromEntity?.Uuid,
                FromEntityName = relation.FromEntity?.Name ?? string.Empty,
                FromEntityType = GetEntityTypeName(relation.FromEntity),
                ToEntityId = relation.ToEntity?.Uuid,
                ToEntityName = relation.ToEntity?.Name ?? string.Empty,
                ToEntityType = GetEntityTypeName(relation.ToEntity),
                RelationType = relation.RelationType,
                Description = relation.Description
            };

            ViewBag.Entities = GetAllEntitiesForPicker(universe);
            ViewBag.RelationTypes = Enum.GetValues<RelationTypeEnum>().Select(e => new { Value = (int)e, Text = e.ToString() });

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading relationship edit form", "Index", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, int relationId, EntityRelationViewModel model)
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
                return RedirectToAction("Index", "Universe");
            }

            var relation = await _entityService.GetRelationByIdAsync(id, relationId);

            if (relation == null || relation.UniverseId != id)
            {
                TempData["ErrorMessage"] = "Relationship not found.";
                return RedirectToAction(nameof(Index), new { id });
            }

            if (!model.FromEntityId.HasValue || !model.ToEntityId.HasValue || !model.RelationType.HasValue)
            {
                ModelState.AddModelError("", "Please select both entities and a relationship type.");
                ViewBag.Entities = GetAllEntitiesForPicker(universe);
                ViewBag.RelationTypes = Enum.GetValues<RelationTypeEnum>().Select(e => new { Value = (int)e, Text = e.ToString() });
                return View(model);
            }

            var (fromEntity, fromType) = FindEntityById(universe, model.FromEntityId.Value);
            var (toEntity, toType) = FindEntityById(universe, model.ToEntityId.Value);

            if (fromEntity == null || toEntity == null)
            {
                ModelState.AddModelError("", "One or both selected entities could not be found.");
                ViewBag.Entities = GetAllEntitiesForPicker(universe);
                ViewBag.RelationTypes = Enum.GetValues<RelationTypeEnum>().Select(e => new { Value = (int)e, Text = e.ToString() });
                return View(model);
            }

            relation.FromEntity = fromEntity;
            relation.ToEntity = toEntity;
            relation.RelationType = model.RelationType.Value;
            relation.UpdatedAt = DateTime.UtcNow;

            await _entityService.UpdateRelationAsync(universe.Uuid, relation);

            TempData["SuccessMessage"] = "Relationship updated successfully.";
            return RedirectToAction(nameof(Index), new { id = universe.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "updating relationship", "Index", "Universe");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, int relationId)
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
                return RedirectToAction("Index", "Universe");
            }

            var relation = await _entityService.GetRelationByIdAsync(id, relationId);

            if (relation == null || relation.UniverseId != id)
            {
                TempData["ErrorMessage"] = "Relationship not found.";
                return RedirectToAction(nameof(Index), new { id });
            }

            await _entityService.DeleteRelationAsync(id, relationId);

            TempData["SuccessMessage"] = "Relationship deleted successfully.";
            return RedirectToAction(nameof(Index), new { id = universe.Uuid });
        }
        catch (Exception ex)
        {
            return HandleException(ex, "deleting relationship", "Index", "Universe");
        }
    }

    private List<EntityPickerItem> GetAllEntitiesForPicker(Universe universe)
    {
        var entities = new List<EntityPickerItem>();

        foreach (var figure in universe.Figures.Where(f => !f.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = figure.Uuid,
                EntityName = figure.Name,
                EntityType = "NotableFigure"
            });
        }

        foreach (var location in universe.Locations.Where(l => !l.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = location.Uuid,
                EntityName = location.Name,
                EntityType = "Location"
            });
        }

        foreach (var artifact in universe.Artifacts.Where(a => !a.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = artifact.Uuid,
                EntityName = artifact.Name,
                EntityType = "Artifact"
            });
        }

        foreach (var evt in universe.CannonEvents.Where(e => !e.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = evt.Uuid,
                EntityName = evt.Name,
                EntityType = "CannonEvent"
            });
        }

        foreach (var faction in universe.Factions.Where(f => !f.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = faction.Uuid,
                EntityName = faction.Name,
                EntityType = "Faction"
            });
        }

        foreach (var fact in universe.Facts.Where(f => !f.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = fact.Uuid,
                EntityName = fact.Name,
                EntityType = "Fact"
            });
        }

        foreach (var species in universe.Species.Where(s => !s.IsDeleted))
        {
            entities.Add(new EntityPickerItem
            {
                EntityId = species.Uuid,
                EntityName = species.Name,
                EntityType = "Species"
            });
        }

        return entities.OrderBy(e => e.EntityType).ThenBy(e => e.EntityName).ToList();
    }

    private (BaseEntity? entity, string type) FindEntityById(Universe universe, Guid entityId)
    {
        var figure = universe.Figures.FirstOrDefault(f => f.Uuid == entityId && !f.IsDeleted);
        if (figure != null) return (figure, "NotableFigure");

        var location = universe.Locations.FirstOrDefault(l => l.Uuid == entityId && !l.IsDeleted);
        if (location != null) return (location, "Location");

        var artifact = universe.Artifacts.FirstOrDefault(a => a.Uuid == entityId && !a.IsDeleted);
        if (artifact != null) return (artifact, "Artifact");

        var evt = universe.CannonEvents.FirstOrDefault(e => e.Uuid == entityId && !e.IsDeleted);
        if (evt != null) return (evt, "CannonEvent");

        var faction = universe.Factions.FirstOrDefault(f => f.Uuid == entityId && !f.IsDeleted);
        if (faction != null) return (faction, "Faction");

        var fact = universe.Facts.FirstOrDefault(f => f.Uuid == entityId && !f.IsDeleted);
        if (fact != null) return (fact, "Fact");

        var species = universe.Species.FirstOrDefault(s => s.Uuid == entityId && !s.IsDeleted);
        if (species != null) return (species, "Species");

        return (null, string.Empty);
    }

    private string GetEntityTypeName(BaseEntity? entity)
    {
        if (entity == null) return "Unknown";

        return entity switch
        {
            NotableFigure => "NotableFigure",
            Location => "Location",
            Artifact => "Artifact",
            CannonEvent => "CannonEvent",
            Faction => "Faction",
            Fact => "Fact",
            Species => "Species",
            _ => "Unknown"
        };
    }
}
