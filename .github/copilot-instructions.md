# Glimmer - Universe Building Tool

## Architecture Overview

Glimmer is a universe building tool for storytelling with a 2-tier architecture:
- **Glimmer.Core**: Domain models, business logic, services, and MongoDB repositories (.NET 8 class library)
- **Glimmer.Creator**: ASP.NET Core MVC web application with dark mode UI (.NET 8 MVC)

## Core Domain Model

The domain centers around **Universe** as the root aggregate containing collections of **7 entity types**:
- `NotableFigure` - Characters/people (19 types: Protagonist, Antagonist, Hero, Villain, etc.)
- `Location` - Places with hierarchical parent-child relationships (11 types)
- `Artifact` - Objects, items, and significant things (19 types: Weapon, Armor, Magic Item, etc.)
- `TimelineEvent` - Historical events and occurrences (20 types: Battle, Birth, Death, etc.)
- `Faction` - Groups, organizations, and political entities (13 types: Kingdom, Guild, etc.)
- `Fact` - Miscellaneous facts, lore, and trivia (11 types: Historical, Scientific, etc.)
- `Species` - Lifeforms and creatures (16 types: Mammal, Reptile, Humanoid, Dragon, etc.)

**Total**: 101 type variants for rich categorization and detailed world-building.
- `Species` - Lifeforms and creatures in the universe

### Entity Patterns

All domain entities inherit from `BaseEntity` which provides:
```csharp
[BsonId]
[BsonElement("_id")]
public Guid Uuid { get; set; } = Guid.NewGuid();
public required string Name { get; set; }
public required string Description { get; set; }
public DateTime CreatedAt/UpdatedAt { get; set; }
public bool IsDeleted { get; set; } = false;
```

**IMPORTANT**: MongoDB BSON attributes are REQUIRED:
- `[BsonId]` - Marks the property as MongoDB document ID
- `[BsonElement("_id")]` - Maps Guid properties to _id field
- `[BsonIgnoreExtraElements]` - Add to all model classes

### Relationship System

Entities are connected via `EntityRelation` which creates typed relationships between any two entities:
- Uses `RelationTypeEnum` for semantic relationships (ParentOf, LocatedIn, ParticipatedIn, etc.)
- Supports both generic (AssociatedWith) and specific (BornIn, RuledOver) relationship types
- Includes family/social relations (ParentOf, AllyOf, EnemyOf)
- Stored in separate MongoDB collection for efficient querying

## Development Workflow

### Build & Run
```bash
# Build entire solution
dotnet build

# Run MVC application (http://localhost:5228)
cd Glimmer.Creator && dotnet run

# Login with Admin / Password1234
```

### MongoDB Setup
```bash
# Start MongoDB with Docker
docker run -d -p 27017:27017 --name mongodb mongo:latest

# Or install locally (Ubuntu/Debian)
sudo apt-get install mongodb-org
sudo systemctl start mongod

# Verify connection
docker exec -it mongodb mongosh
use GlimmerDB
db.users.find().pretty()
```

## Project Conventions

### Naming Patterns
- Enum suffix: `TypeEnum` (e.g., `FigureTypeEnum`, `RelationTypeEnum`)
- Model inheritance: Domain entities extend `BaseEntity`
- Required properties: Use `required` keyword for essential fields
- Repositories: `I{Entity}Repository` interface, `{Entity}Repository` implementation
- Services: `I{Service}` interface, `{Service}` implementation
- Controllers: Inherit from `BaseController` for shared functionality

### MongoDB Integration
- **All repositories are ALREADY implemented** ✅
- **All services are ALREADY migrated to MongoDB** ✅
- Entities use `Guid Uuid` as primary key (mapped to MongoDB `_id`)
- Soft delete pattern via `IsDeleted` flag
- Embedded documents: Entities stored within Universe
- Separate collections: Users, Relations, Tokens

### Repository Pattern
```csharp
// Example repository usage (already implemented)
var user = await _userRepository.GetByUsernameAsync(username);
var universe = await _universeRepository.CreateAsync(newUniverse);
var relations = await _relationRepository.GetByUniverseIdAsync(universeId);

// Universe updates (entities are embedded)
var universe = await _universeRepository.GetByIdAsync(universeId);
universe.Figures.Add(newFigure);
universe.UpdatedAt = DateTime.UtcNow;
await _universeRepository.UpdateAsync(universe);
```

### Modular Service Architecture
EntityService uses **partial classes** split across 11 files for maintainability:
- `EntityService.cs` - Main class with DI (40 lines)
- `IEntityService.cs` - Interface definition (85 lines)
- `EntityService.Universe.cs` - Universe operations (100 lines)
- `EntityService.Artifact.cs` - Artifact CRUD (~85 lines)
- `EntityService.TimelineEvent.cs` - Event CRUD (~85 lines)
- `EntityService.Faction.cs` - Faction CRUD (~85 lines)
- `EntityService.Location.cs` - Location CRUD with hierarchy (~85 lines)
- `EntityService.NotableFigure.cs` - Character CRUD (~85 lines)
- `EntityService.Fact.cs` - Fact CRUD (~85 lines)
- `EntityService.Species.cs` - Species CRUD (~85 lines)
- `EntityService.Relations.cs` - Relationship operations (~85 lines)
- `EntityService.Generic.cs` - Search and count operations (~120 lines)

See `Glimmer.Core/Services/README.md` for details.

### MVC Web Application
- ASP.NET Core MVC with Razor views
- Dark mode always enabled (#1a1a1a background, #e0e0e0 text, #9333ea accents)
- File ribbon menu at top with cascading submenus
- Bootstrap 5.3 for responsive design
- HttpOnly cookies for JWT tokens
- BaseController provides shared methods for authentication, validation, error handling
- Serilog structured logging (Console, File, MongoDB sinks)
- Global exception middleware with friendly error pages

### Full CRUD Controllers (All Implemented) ✅
- UniverseController - Universe management with dashboard
- NotableFigureController - Character management (19 types)
- LocationController - Location management with hierarchy (11 types)
- ArtifactController - Artifact management (19 types)
- TimelineEventController - Event management (20 types)
- FactionController - Faction management (13 types)
- FactController - Fact/lore management (11 types)
- SpeciesController - Species management (16 types)

Each controller inherits from BaseController and includes:
- Authorization checks
- Full CRUD operations (List, Create, Edit, Delete, Details)
- Type-specific badges with color coding
- Breadcrumb navigation
- Delete confirmation modals
- Integration with Universe dashboard

## Current Implementation Status

### ✅ Completed (100%)
- MongoDB infrastructure
  - MongoDbSettings configuration
  - Repository implementations (User, Token, Universe, Relation)
  - Automatic index creation
  - BSON serialization with attributes
- Services migrated to MongoDB
  - AuthenticationService (13 methods)
  - EntityService (70+ methods in 11 modular partial classes)
- Authentication system
  - JWT access tokens (60 min)
  - Refresh tokens (7 days)
  - Password hashing (HMACSHA512)
  - Superuser seeding (Admin/Password1234)
- Logging system
  - Serilog structured logging (Console, File, MongoDB)
  - User context enrichment
  - Request logging middleware
- Error handling
  - Global exception middleware
  - BaseController with HandleException helper
  - Friendly error pages
- Full MVC UI
  - Dark mode layout with file ribbon navigation
  - AccountController (Login, Register, Password Reset)
  - HomeController (Dashboard with universe cards)
  - Universe CRUD with dashboard
  - All 7 entity types with full CRUD (101 type variants)
  - Responsive design with Bootstrap 5.3
  - Type-specific badges and color coding

### 🚧 In Progress (10%)
- Relationship management UI
- Unit testing framework (skeleton created)

### ⏳ Not Started
- Search functionality
- Visualization (graphs, timelines)
- Export/import
- Advanced filtering

## Key Files for Understanding
- `Glimmer.Core/Models/BaseEntity.cs` - Common entity pattern with BSON attributes
- `Glimmer.Core/Models/Universe.cs` - Root aggregate with embedded entities
- `Glimmer.Core/Models/EntityRelation.cs` - Relationship modeling
- `Glimmer.Core/Enums/RelationTypeEnum.cs` - Semantic relationship types
- `Glimmer.Core/Repositories/*.cs` - MongoDB repository implementations
- `Glimmer.Core/Services/IEntityService.cs` - Service interface (85 lines)
- `Glimmer.Core/Services/EntityService.cs` - Main service class with DI (40 lines)
- `Glimmer.Core/Services/EntityService.*.cs` - 10 partial classes by entity type (~85 lines each)
- `Glimmer.Core/Services/README.md` - Modular EntityService architecture guide
- `Glimmer.Core/Services/AuthenticationService.cs` - Authentication logic (MongoDB)
- `Glimmer.Core/Extensions/ServiceCollectionExtensions.cs` - DI registration
- `Glimmer.Creator/Program.cs` - MVC application startup & superuser seeding
- `Glimmer.Creator/Controllers/BaseController.cs` - Shared controller functionality
- `Glimmer.Creator/Controllers/AccountController.cs` - Authentication flows
- `Glimmer.Creator/Controllers/UniverseController.cs` - Universe CRUD with dashboard
- `Glimmer.Creator/Controllers/*Controller.cs` - Entity-specific CRUD controllers
- `Glimmer.Creator/Views/Shared/_Layout.cshtml` - Dark mode layout with file ribbon
- `Glimmer.Creator/Views/Shared/_FileRibbon.cshtml` - Navigation menu component
- `Glimmer.Core/Services/EntityService.cs` - Entity management (MongoDB)
- `Glimmer.Core/Extensions/ServiceCollectionExtensions.cs` - DI registration
- `Glimmer.Creator/Program.cs` - MVC application startup
- `Glimmer.Creator/Views/Shared/_Layout.cshtml` - Dark mode layout

## Common Development Tasks

### Adding a New Controller
1. Create controller in `Glimmer.Creator/Controllers/` **inheriting from `BaseController`**
2. Inject required services (`IEntityService`, `IAuthenticationService`, `ILogger`)
3. Pass logger to base constructor: `public MyController(ILogger<MyController> logger) : base(logger)`
4. Use base controller helper methods:
   - `GetCurrentUserId()`, `GetCurrentUsername()`, `GetCurrentUserIdAsGuid()`
   - `IsAuthenticated()`, `RedirectToLogin()`
   - `ValidatePasswordRequirements(password, confirmPassword, out errorMessage)`
   - `SetAuthenticationCookieAndSession()`, `ClearAuthenticationCookieAndSession()`
   - `HandleException(ex, operation, redirectAction, redirectController)`
5. Create corresponding views in `Glimmer.Creator/Views/{ControllerName}/`
6. Add navigation links in `_Layout.cshtml`
7. Create view models if needed

**Example:**
```csharp
public class UniverseController : BaseController
{
    private readonly IEntityService _entityService;
    
    public UniverseController(IEntityService entityService, ILogger<UniverseController> logger) 
        : base(logger)
    {
        _entityService = entityService;
    }
    
    public async Task<IActionResult> Index()
    {
        if (!IsAuthenticated())
        {
            return RedirectToLogin();
        }
        
        try
        {
            var userId = GetCurrentUserIdAsGuid()!.Value;
            var universes = await _entityService.GetAllUniversesAsync(userId);
            return View(universes);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading universes", "Index", "Home");
        }
    }
}
```

### Adding a New Entity Type
1. Create model in `Glimmer.Core/Models/` inheriting `BaseEntity`
2. Add BSON attributes (`[BsonIgnoreExtraElements]` on class)
3. Add enum for entity type in `Glimmer.Core/Enums/`
4. Add collection property to `Universe.cs` model
5. Services already support generic entity operations

### Working with Repositories
All repositories are already implemented. Use them via dependency injection:

```csharp
// Controllers should inherit from BaseController
public class UniverseController : BaseController
{
    private readonly IEntityService _entityService;
    
    public UniverseController(IEntityService entityService, ILogger<UniverseController> logger)
        : base(logger)
    {
        _entityService = entityService;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = GetCurrentUserIdAsGuid()!.Value; // From BaseController
            var universes = await _entityService.GetAllUniversesAsync(userId);
            return View(universes);
        }
        catch (Exception ex)
        {
            return HandleException(ex, "loading universes", "Index", "Home");
        }
    }
}
```

### Working with MongoDB Directly (Rare)
```csharp
// Only if repository doesn't support your use case
private readonly IMongoDatabase _database;

var collection = _database.GetCollection<Universe>("universes");
var universe = await collection.Find(u => u.Uuid == id).FirstOrDefaultAsync();
```

## Integration Points
- MVC application references Core project
- Controllers use services (not repositories directly)
- Services use repositories for data access
- MongoDB connection configured in `appsettings.json`
- Superuser seeded automatically on startup

## Testing Guidelines
- Unit tests should mock repositories
- Integration tests should use test database
- Test data should not use production database
- Use `[Fact]` for xUnit tests

## Security Best Practices
- **NEVER** store passwords in plain text
- **ALWAYS** use HTTPS in production
- **ALWAYS** validate user input
- **NEVER** expose JWT secret in code
- **ALWAYS** use HttpOnly cookies for tokens
- Change default superuser password immediately

## Common Pitfalls to Avoid
1. ❌ Forgetting BSON attributes on new models
2. ❌ Not calling `UpdateAsync` after modifying entities
3. ❌ Using LINQ on MongoDB collections without `AsQueryable()`
4. ❌ Not handling null returns from repository queries
5. ❌ Exposing repository interfaces outside of services
6. ❌ Forgetting to update `Universe.UpdatedAt` when modifying entities
7. ❌ Not inheriting controllers from `BaseController`
8. ❌ Creating monolithic service files (use partial classes like EntityService)

## Next Steps (Priority Order)
1. 🔴 Implement RelationController for entity relationship management
2. 🔴 Add unit tests for EntityService partial classes
3. 🟡 Implement global search functionality
4. 🟡 Add relationship graph visualization
5. 🟡 Implement timeline view for TimelineEvents
6. 🟡 Add data export/import (JSON/YAML)

See [TODO.md](TODO.md) for complete task list.