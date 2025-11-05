# EntityService Architecture

The EntityService has been refactored into **11 smaller, focused files** using C# partial classes for better maintainability and development experience.

## File Structure

```
Glimmer.Core/Services/
├── IEntityService.cs               # Service interface definition
├── EntityService.cs                # Main class with DI and fields
├── EntityService.Universe.cs       # Universe CRUD operations
├── EntityService.Artifact.cs       # Artifact entity operations  
├── EntityService.TimelineEvent.cs    # Event entity operations
├── EntityService.Faction.cs        # Faction entity operations
├── EntityService.Location.cs       # Location operations (with hierarchy)
├── EntityService.NotableFigure.cs  # Character entity operations
├── EntityService.Fact.cs           # Fact/lore entity operations
├── EntityService.Species.cs        # Species entity operations
├── EntityService.Relations.cs      # Entity relationship operations
└── EntityService.Generic.cs        # Generic search and count operations
```

## Benefits of Partial Classes

✅ **Easier Navigation** - Find specific entity logic quickly  
✅ **Reduced Merge Conflicts** - Team members can work on different entity types simultaneously  
✅ **Better Code Organization** - Each file focuses on a single entity type (~50-100 lines)  
✅ **Improved Readability** - No more scrolling through 860+ lines  
✅ **Logical Grouping** - Related operations stay together  

## File Responsibilities

### EntityService.cs (Main)
- Dependency injection setup
- Shared fields (_universeRepository, _relationRepository, _logger)
- No business logic - just infrastructure

### Entity-Specific Files
Each file contains CRUD operations for one entity type:
- `Create{Entity}Async` - Create new entity
- `Get{Entity}ByIdAsync` - Retrieve by ID
- `Get{Entity}sByUniverseAsync` - List all in universe
- `Update{Entity}Async` - Update existing entity
- `Delete{Entity}Async` - Soft delete entity

### EntityService.Relations.cs
- CreateRelationAsync
- GetRelationsByEntityAsync
- UpdateRelationAsync  
- DeleteRelationAsync

### EntityService.Generic.cs
- GetEntityByIdAsync - Retrieve any entity type by ID
- SearchEntitiesAsync - Full-text search across all entities
- GetEntityCountAsync - Count all entities in universe

## Usage

The service is used exactly the same way as before - the partial class mechanism is transparent to consumers:

```csharp
public class MyController : Controller
{
    private readonly IEntityService _entityService;
    
    public MyController(IEntityService entityService)
    {
        _entityService = entityService; // Still just one interface!
    }
    
    public async Task<IActionResult> Create()
    {
        // All methods available on the single interface
        var figure = await _entityService.CreateNotableFigureAsync(...);
        var location = await _entityService.CreateLocationAsync(...);
        // etc.
    }
}
```

## Development Guidelines

### Adding New Methods
1. Determine which entity type the method belongs to
2. Add method signature to `IEntityService.cs`
3. Implement in the appropriate `EntityService.{EntityType}.cs` file
4. Use existing methods in that file as a template
5. Access shared fields (`_universeRepository`, `_logger`) directly

### Creating New Entity Types
1. Add interface methods to `IEntityService.cs`
2. Create new file `EntityService.{NewEntity}.cs`
3. Follow the pattern of existing entity files:
   ```csharp
   using Glimmer.Core.Models;
   using Microsoft.Extensions.Logging;
   using Glimmer.Core.Enums;
   
   namespace Glimmer.Core.Services;
   
   /// <summary>
   /// EntityService partial class - NewEntity operations
   /// </summary>
   public partial class EntityService
   {
       // CRUD methods here
   }
   ```

### Refactoring Tips
- Keep each partial file under 150 lines if possible
- Group related helper methods with their entity
- Use consistent naming patterns across entity types
- Add XML documentation comments for public methods

## Testing

Tests remain unchanged - they mock `IEntityService` and test methods normally. The partial class implementation is transparent to unit tests.

## Performance

No performance impact - partial classes are a compile-time feature. The compiler combines all partial class files into a single class at build time.

## Migration Notes

**Before:** Single 860-line file  
**After:** 11 focused files (40-100 lines each)  

No breaking changes - all existing code continues to work without modification.
