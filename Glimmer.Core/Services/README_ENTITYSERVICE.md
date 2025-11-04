# EntityService Documentation

## Overview
The `EntityService` provides comprehensive CRUD (Create, Read, Update, Delete) operations for all entity types in the Glimmer universe building system. It manages Universes and all their contained entities including Artifacts, CannonEvents, Factions, Locations, NotableFigures, Facts, and EntityRelations.

## Architecture

### In-Memory Storage
The service currently uses in-memory collections for development:
- `List<Universe> _universes` - All universes
- `Dictionary<Guid, List<EntityRelation>> _relations` - Relations per universe

**Note:** Replace with MongoDB repositories for production use.

## Entity Types Supported

1. **Universe** - Root aggregate containing all entities
2. **Artifact** - Objects, items, and significant things
3. **CannonEvent** - Historical events and occurrences
4. **Faction** - Groups, organizations, and political entities
5. **Location** - Places and geographical entities (supports hierarchical relationships)
6. **NotableFigure** - Characters and people
7. **Fact** - Facts and data points with values
8. **EntityRelation** - Relationships between any two entities

## Service Interface

### Universe Operations

```csharp
// Create a new universe
Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy);

// Get universe by ID
Task<Universe?> GetUniverseByIdAsync(Guid universeId);

// Get all universes for a specific user
Task<List<Universe>> GetUniversesByUserAsync(Guid userId);

// Get all universes
Task<List<Universe>> GetAllUniversesAsync();

// Update universe properties
Task<bool> UpdateUniverseAsync(Universe universe);

// Delete universe (hard delete with all contained entities)
Task<bool> DeleteUniverseAsync(Guid universeId);
```

### Artifact Operations

```csharp
Task<Artifact?> CreateArtifactAsync(Guid universeId, string name, string description, ArtifactTypeEnum artifactType);
Task<Artifact?> GetArtifactByIdAsync(Guid universeId, Guid artifactId);
Task<List<Artifact>> GetArtifactsByUniverseAsync(Guid universeId);
Task<bool> UpdateArtifactAsync(Guid universeId, Artifact artifact);
Task<bool> DeleteArtifactAsync(Guid universeId, Guid artifactId); // Soft delete
```

### CannonEvent Operations

```csharp
Task<CannonEvent?> CreateCannonEventAsync(Guid universeId, string name, string description, CannonEventTypeEnum eventType);
Task<CannonEvent?> GetCannonEventByIdAsync(Guid universeId, Guid eventId);
Task<List<CannonEvent>> GetCannonEventsByUniverseAsync(Guid universeId);
Task<bool> UpdateCannonEventAsync(Guid universeId, CannonEvent cannonEvent);
Task<bool> DeleteCannonEventAsync(Guid universeId, Guid eventId); // Soft delete
```

### Faction Operations

```csharp
Task<Faction?> CreateFactionAsync(Guid universeId, string name, string description, FactionTypeEnum factionType);
Task<Faction?> GetFactionByIdAsync(Guid universeId, Guid factionId);
Task<List<Faction>> GetFactionsByUniverseAsync(Guid universeId);
Task<bool> UpdateFactionAsync(Guid universeId, Faction faction);
Task<bool> DeleteFactionAsync(Guid universeId, Guid factionId); // Soft delete
```

### Location Operations

```csharp
Task<Location?> CreateLocationAsync(Guid universeId, string name, string description, LocationTypeEnum locationType, Location? parentLocation = null);
Task<Location?> GetLocationByIdAsync(Guid universeId, Guid locationId);
Task<List<Location>> GetLocationsByUniverseAsync(Guid universeId);
Task<bool> UpdateLocationAsync(Guid universeId, Location location);
Task<bool> DeleteLocationAsync(Guid universeId, Guid locationId); // Soft delete
```

### NotableFigure Operations

```csharp
Task<NotableFigure?> CreateNotableFigureAsync(Guid universeId, string name, string description, FigureTypeEnum figureType);
Task<NotableFigure?> GetNotableFigureByIdAsync(Guid universeId, Guid figureId);
Task<List<NotableFigure>> GetNotableFiguresByUniverseAsync(Guid universeId);
Task<bool> UpdateNotableFigureAsync(Guid universeId, NotableFigure figure);
Task<bool> DeleteNotableFigureAsync(Guid universeId, Guid figureId); // Soft delete
```

### Fact Operations

```csharp
Task<Fact?> CreateFactAsync(Guid universeId, string name, string description, string value, FactTypeEnum factType);
Task<Fact?> GetFactByIdAsync(Guid universeId, Guid factId);
Task<List<Fact>> GetFactsByUniverseAsync(Guid universeId);
Task<bool> UpdateFactAsync(Guid universeId, Fact fact);
Task<bool> DeleteFactAsync(Guid universeId, Guid factId); // Soft delete
```

### EntityRelation Operations

```csharp
Task<EntityRelation?> CreateRelationAsync(Guid universeId, BaseEntity fromEntity, BaseEntity toEntity, RelationTypeEnum relationType);
Task<EntityRelation?> GetRelationByIdAsync(Guid universeId, int relationId);
Task<List<EntityRelation>> GetRelationsByUniverseAsync(Guid universeId);
Task<List<EntityRelation>> GetRelationsByEntityAsync(Guid universeId, Guid entityId);
Task<bool> UpdateRelationAsync(Guid universeId, EntityRelation relation);
Task<bool> DeleteRelationAsync(Guid universeId, int relationId); // Soft delete
```

### Generic BaseEntity Operations

```csharp
// Get any entity by ID (searches all entity types)
Task<BaseEntity?> GetEntityByIdAsync(Guid universeId, Guid entityId);

// Search across all entity types
Task<List<BaseEntity>> SearchEntitiesAsync(Guid universeId, string searchTerm);

// Get total count of all entities in a universe
Task<int> GetEntityCountAsync(Guid universeId);
```

## Usage Examples

### 1. Create a Universe and Add Entities

```csharp
var entityService = serviceProvider.GetRequiredService<IEntityService>();
var authService = serviceProvider.GetRequiredService<IAuthenticationService>();

// Get or create a user
var loginResult = await authService.LoginAsync("Admin", "Password1234");
var user = loginResult.User;

// Create a universe
var universe = await entityService.CreateUniverseAsync(
    "Middle Earth",
    "Tolkien's fantasy world",
    user
);

if (universe != null)
{
    // Add a location
    var location = await entityService.CreateLocationAsync(
        universe.Uuid,
        "The Shire",
        "A peaceful land inhabited by hobbits",
        LocationTypeEnum.Region
    );

    // Add a notable figure
    var figure = await entityService.CreateNotableFigureAsync(
        universe.Uuid,
        "Frodo Baggins",
        "A hobbit tasked with destroying the One Ring",
        FigureTypeEnum.Protagonist
    );

    // Add an artifact
    var artifact = await entityService.CreateArtifactAsync(
        universe.Uuid,
        "The One Ring",
        "A powerful ring of invisibility",
        ArtifactTypeEnum.MagicalItem
    );

    // Create a relationship
    if (figure != null && location != null)
    {
        var relation = await entityService.CreateRelationAsync(
            universe.Uuid,
            figure,
            location,
            RelationTypeEnum.BornIn
        );
    }
}
```

### 2. Update an Entity

```csharp
var artifact = await entityService.GetArtifactByIdAsync(universeId, artifactId);

if (artifact != null)
{
    artifact.Name = "The One Ring of Power";
    artifact.Description = "Updated description with more detail";
    artifact.ArtifactType = ArtifactTypeEnum.Legendary;

    var success = await entityService.UpdateArtifactAsync(universeId, artifact);
}
```

### 3. Search for Entities

```csharp
// Search across all entity types
var results = await entityService.SearchEntitiesAsync(universeId, "ring");

foreach (var entity in results)
{
    Console.WriteLine($"{entity.GetType().Name}: {entity.Name}");
}
```

### 4. Get All Entities of a Type

```csharp
var figures = await entityService.GetNotableFiguresByUniverseAsync(universeId);
var locations = await entityService.GetLocationsByUniverseAsync(universeId);
var artifacts = await entityService.GetArtifactsByUniverseAsync(universeId);
```

### 5. Create Hierarchical Locations

```csharp
// Create a parent location
var continent = await entityService.CreateLocationAsync(
    universeId,
    "Middle Earth",
    "The main continent",
    LocationTypeEnum.Continent
);

// Create a child location
var region = await entityService.CreateLocationAsync(
    universeId,
    "The Shire",
    "A region in Middle Earth",
    LocationTypeEnum.Region,
    continent // Parent location
);
```

### 6. Work with Relations

```csharp
// Get all relations for an entity
var entityRelations = await entityService.GetRelationsByEntityAsync(
    universeId,
    figureId
);

foreach (var relation in entityRelations)
{
    Console.WriteLine(relation.Description);
    // Output: "Frodo Baggins - BornIn -> The Shire"
}

// Get all relations in a universe
var allRelations = await entityService.GetRelationsByUniverseAsync(universeId);
```

### 7. Delete Entities (Soft Delete)

```csharp
// Entities are soft-deleted (IsDeleted = true)
var deleted = await entityService.DeleteArtifactAsync(universeId, artifactId);

// The entity still exists but won't appear in queries
var artifact = await entityService.GetArtifactByIdAsync(universeId, artifactId);
// Returns null because IsDeleted = true
```

### 8. Get Entity Statistics

```csharp
var totalEntities = await entityService.GetEntityCountAsync(universeId);
Console.WriteLine($"Total entities in universe: {totalEntities}");
```

## Entity Lifecycle

### Creation
1. Entity is created with a new Guid UUID
2. CreatedAt and UpdatedAt are set to current UTC time
3. Entity is added to the appropriate collection in the Universe
4. Universe's UpdatedAt is updated

### Update
1. Existing entity is retrieved
2. Properties are updated
3. Entity's UpdatedAt is set to current UTC time
4. Universe's UpdatedAt is updated

### Deletion (Soft Delete)
1. Entity's IsDeleted flag is set to true
2. Entity's UpdatedAt is set to current UTC time
3. Universe's UpdatedAt is updated
4. Entity is filtered out of all query results

## Soft Delete Pattern

All entities (except Universe) use soft deletes:
- `IsDeleted` flag is set to `true`
- Entity remains in database
- Filtered out of all Get/Search operations
- Can be recovered by setting `IsDeleted = false`

### Why Soft Delete?
- Maintains referential integrity
- Enables audit trails
- Allows "undo" functionality
- Preserves relationships

## Service Registration

Register the service in `Program.cs`:

```csharp
builder.Services.AddGlimmerCore(); // Already includes EntityService
```

Or manually:

```csharp
builder.Services.AddScoped<IEntityService, EntityService>();
```

## Production Migration

### MongoDB Implementation

Replace in-memory collections with MongoDB:

```csharp
public class EntityService : IEntityService
{
    private readonly IMongoCollection<Universe> _universes;
    private readonly IMongoCollection<EntityRelation> _relations;

    public EntityService(IMongoDatabase database)
    {
        _universes = database.GetCollection<Universe>("universes");
        _relations = database.GetCollection<EntityRelation>("relations");
    }

    public async Task<Universe?> GetUniverseByIdAsync(Guid universeId)
    {
        return await _universes
            .Find(u => u.Uuid == universeId)
            .FirstOrDefaultAsync();
    }

    // ... implement other methods with MongoDB queries
}
```

### Recommended Indexes

```javascript
// MongoDB indexes for optimal performance
db.universes.createIndex({ "Uuid": 1 }, { unique: true });
db.universes.createIndex({ "CreatedBy.Uuid": 1 });

db.relations.createIndex({ "FromEntity.Uuid": 1 });
db.relations.createIndex({ "ToEntity.Uuid": 1 });
db.relations.createIndex({ "IsDeleted": 1 });
```

## Error Handling

Methods return `null` when:
- Universe not found
- Entity not found
- Entity is soft-deleted

Methods return `false` when:
- Update fails (entity not found)
- Delete fails (entity not found)

## Best Practices

1. **Always check for null returns**
   ```csharp
   var entity = await entityService.GetArtifactByIdAsync(universeId, artifactId);
   if (entity == null)
   {
       // Handle not found
   }
   ```

2. **Use transactions in production**
   - Wrap multi-entity operations in transactions
   - Ensures data consistency

3. **Validate before operations**
   - Check universe exists before creating entities
   - Validate entity types match operation

4. **Handle concurrent updates**
   - Implement optimistic concurrency
   - Use UpdatedAt timestamp for conflict detection

5. **Clean up relations on delete**
   - Consider cascading deletes
   - Remove relations when entities are deleted

## Testing

```csharp
[Fact]
public async Task CreateArtifact_ValidData_ReturnsArtifact()
{
    // Arrange
    var service = new EntityService();
    var user = new User { /* ... */ };
    var universe = await service.CreateUniverseAsync("Test", "Test Universe", user);

    // Act
    var artifact = await service.CreateArtifactAsync(
        universe!.Uuid,
        "Excalibur",
        "The legendary sword",
        ArtifactTypeEnum.Weapon
    );

    // Assert
    Assert.NotNull(artifact);
    Assert.Equal("Excalibur", artifact.Name);
}
```

## Related Files

- `Glimmer.Core/Models/*.cs` - Entity models
- `Glimmer.Core/Enums/*.cs` - Type enumerations
- `Glimmer.Core/Extensions/ServiceCollectionExtensions.cs` - DI registration

## Future Enhancements

- [ ] Add batch operations (CreateMany, UpdateMany, DeleteMany)
- [ ] Implement change tracking
- [ ] Add versioning for entities
- [ ] Support for attachments/media
- [ ] Full-text search integration
- [ ] Export/import functionality
- [ ] Validation rules engine
- [ ] Event sourcing support
- [ ] Caching layer
- [ ] Pagination for large result sets
