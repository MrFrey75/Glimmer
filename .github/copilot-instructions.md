# Glimmer - Universe Building Tool

## Architecture Overview

Glimmer is a universe building tool for storytelling with a 2-tier architecture:
- **Glimmer.Core**: Domain models and business logic (.NET 8 class library with MongoDB)
- **Glimmer.Creator**: MVC web application (.NET 8 MVC with Razor views)

## Core Domain Model

The domain centers around **Universe** as the root aggregate containing collections of:
- `NotableFigure` - Characters/people in the universe
- `Location` - Places and geographical entities
- `Artifact` - Objects, items, and significant things
- `CannonEvent` - Historical events and occurrences
- `Faction` - Groups, organizations, and political entities
- `Fact` - Miscellaneous facts, lore and trivia

### Entity Patterns

All domain entities inherit from `BaseEntity` which provides:
```csharp
public Guid Uuid { get; set; } = Guid.NewGuid();
public required string Name { get; set; }
public required string Description { get; set; }
public DateTime CreatedAt/UpdatedAt { get; set; }
public bool IsDeleted { get; set; } = false;
```

### Relationship System

Entities are connected via `EntityRelation` which creates typed relationships between any two entities:
- Uses `RelationTypeEnum` for semantic relationships (ParentOf, LocatedIn, ParticipatedIn, etc.)
- Supports both generic (AssociatedWith) and specific (BornIn, RuledOver) relationship types
- Includes family/social relations (ParentOf, AllyOf, EnemyOf)

## Development Workflow

### Build Solution
```bash
# Build entire solution
dotnet build

# Run MVC application
cd Glimmer.Creator && dotnet run

# The MVC app serves views and interacts directly with Glimmer.Core
```

## Project Conventions

### Naming Patterns
- Enum suffix: `TypeEnum` (e.g., `FigureTypeEnum`, `RelationTypeEnum`)
- Model inheritance: Domain entities extend `BaseEntity`
- Required properties: Use `required` keyword for essential fields

### MongoDB Integration
- Glimmer.Core references `MongoDB.Driver` package
- Entities use `Guid Uuid` as primary key (not MongoDB ObjectId)
- Soft delete pattern via `IsDeleted` flag

### MVC Web Application
- ASP.NET Core MVC with Razor views
- References Glimmer.Core for domain models
- Provides web interface for universe building

## Key Files for Understanding
- `Glimmer.Core/Models/Universe.cs` - Root aggregate and entity collections
- `Glimmer.Core/Models/BaseEntity.cs` - Common entity pattern
- `Glimmer.Core/Models/EntityRelation.cs` - Relationship modeling
- `Glimmer.Core/Enums/RelationTypeEnum.cs` - Semantic relationship types
- `Glimmer.Creator/Program.cs` - MVC application configuration

## Integration Points
- MVC application references Core project for domain models
- Controllers interact directly with domain models
- MongoDB for persistence (connection configuration in MVC app)