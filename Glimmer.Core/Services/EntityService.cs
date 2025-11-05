using Glimmer.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Glimmer.Core.Services;

/// <summary>
/// Main EntityService class - handles dependency injection and shared fields.
/// Implementation split across partial classes for maintainability:
/// - EntityService.Universe.cs - Universe CRUD operations
/// - EntityService.Artifact.cs - Artifact entity operations  
/// - EntityService.TimelineEvent.cs - Event entity operations
/// - EntityService.Faction.cs - Faction entity operations
/// - EntityService.Location.cs - Location entity operations (with hierarchy)
/// - EntityService.NotableFigure.cs - Character entity operations
/// - EntityService.Fact.cs - Fact/lore entity operations
/// - EntityService.Species.cs - Species entity operations
/// - EntityService.Relations.cs - Entity relationship operations
/// - EntityService.Generic.cs - Generic search and count operations
/// </summary>
public partial class EntityService : IEntityService
{
    private readonly IUniverseRepository _universeRepository;
    private readonly IRelationRepository _relationRepository;
    private readonly ILogger<EntityService> _logger;

    public EntityService(
        IUniverseRepository universeRepository, 
        IRelationRepository relationRepository,
        ILogger<EntityService> logger)
    {
        _universeRepository = universeRepository;
        _relationRepository = relationRepository;
        _logger = logger;
    }
}
