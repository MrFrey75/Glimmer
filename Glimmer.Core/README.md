# Glimmer.Core - Domain Layer

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![MongoDB](https://img.shields.io/badge/MongoDB-2.22.0-green.svg)](https://www.mongodb.com/)

The core domain library for Glimmer universe building tool. Contains all business logic, domain models, services, and data access abstractions.

## 🏗️ Architecture

Glimmer.Core follows Domain-Driven Design (DDD) principles with clean architecture:

```
Glimmer.Core/
├── Models/           # Domain entities and aggregates
├── Enums/           # Domain enumerations and value objects
├── Services/        # Application services and business logic
├── Data/            # Data access abstractions
├── Configuration/   # Configuration models
└── Extensions/      # Service registration extensions
```

## 📋 Domain Models

### Core Entities

All entities inherit from `BaseEntity`:
```csharp
public class BaseEntity
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
```

### Universe Aggregate Root
- **Universe**: Root aggregate containing all universe entities
- **NotableFigure**: Characters, people, and personalities
- **Location**: Places, regions, and geographical entities
- **Artifact**: Objects, items, tools, and significant things
- **CannonEvent**: Historical events, battles, discoveries
- **Faction**: Groups, organizations, nations, and political entities
- **Fact**: Miscellaneous lore, trivia, and universe facts

### Relationship System
```csharp
public class EntityRelation
{
    public required BaseEntity FromEntity { get; set; }
    public required BaseEntity ToEntity { get; set; }
    public RelationTypeEnum RelationType { get; set; }
    // Semantic relationships like ParentOf, LocatedIn, AllyOf, etc.
}
```

### User Management
- **User**: User accounts with authentication data
- **RefreshToken**: JWT refresh token management
- **PasswordResetToken**: Password reset workflow

## 🎯 Services

### Authentication Service (`IAuthenticationService`)
Comprehensive user authentication and authorization:

```csharp
// User Management
Task<AuthenticationResult> RegisterAsync(string username, string email, string password);
Task<AuthenticationResult> LoginAsync(string usernameOrEmail, string password);
Task<User?> GetUserByIdAsync(Guid userId);

// Token Management
Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
Task<bool> RevokeTokenAsync(string refreshToken, string? reason = null);

// Password Management
Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
Task<string?> GeneratePasswordResetTokenAsync(string email);
Task<bool> ResetPasswordAsync(string token, string newPassword);

// Security
string HashPassword(string password, out string salt);
bool VerifyPassword(string password, string hash, string salt);
```

**Features:**
- JWT access tokens (60 min default) + refresh tokens (7 days)
- HMACSHA512 password hashing with salts
- Secure password reset with time-limited tokens
- Token rotation and revocation
- Email verification support

### Entity Service (`IEntityService`)
Generic entity management service for CRUD operations across all domain entities.

## 🗄️ Data Access

### MongoDB Integration
```csharp
public class GlimmerDbContext
{
    public IMongoCollection<Universe> Universes { get; }
    // Additional collections for users, tokens, etc.
}
```

**Features:**
- MongoDB driver integration
- GUID-based primary keys (not ObjectId)
- Soft delete pattern implementation
- Flexible schema evolution support

## ⚙️ Configuration

### JWT Settings
```csharp
public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
```

### Service Registration
```csharp
// In consuming applications
builder.Services.AddGlimmerCore();
```

## 🔧 Dependencies

### NuGet Packages
- **MongoDB.Driver** (2.22.0) - MongoDB integration
- **System.IdentityModel.Tokens.Jwt** (8.0.0) - JWT token generation
- **Microsoft.Extensions.Configuration.Abstractions** (8.0.0) - Configuration
- **Microsoft.Extensions.DependencyInjection** (8.0.0) - DI container

## 🎨 Domain Enumerations

### Entity Types
- `FigureTypeEnum` - Person, Deity, Creature, etc.
- `LocationTypeEnum` - City, Region, Building, etc.
- `ArtifactTypeEnum` - Weapon, Tool, Relic, etc.
- `CannonEventTypeEnum` - Battle, Discovery, Birth, Death, etc.
- `FactionTypeEnum` - Nation, Guild, Family, etc.
- `FactTypeEnum` - Lore, Rule, Custom, etc.

### Relationship Types
```csharp
public enum RelationTypeEnum
{
    // Spatial
    LocatedIn, OccurredAt,
    
    // Ownership  
    CreatedBy, OwnedBy, DiscoveredBy,
    
    // Life Events
    BornIn, DiedIn, ParticipatedIn,
    
    // Social Relations
    ParentOf, ChildOf, SpouseOf, SiblingOf,
    AllyOf, EnemyOf, TeacherOf, StudentOf,
    
    // Influence
    RuledOver, InfluencedBy, ContemporaryOf,
    
    // Generic
    AssociatedWith, Other
}
```

## 🛡️ Security Features

### Password Security
- HMACSHA512 hashing algorithm
- Cryptographically secure salt generation
- Configurable hash iterations
- Secure password verification

### Token Security
- Cryptographically secure token generation
- Token rotation on refresh
- Revocation reason tracking
- Configurable expiration times

### Data Protection
- Soft delete pattern for data recovery
- Audit trail with CreatedAt/UpdatedAt timestamps
- Account activation/deactivation support

## 🧪 Usage Examples

### User Registration
```csharp
var authService = serviceProvider.GetRequiredService<IAuthenticationService>();
var result = await authService.RegisterAsync("johndoe", "john@example.com", "SecurePass123!");

if (result.Success)
{
    var user = result.User;
    var accessToken = result.AccessToken;
    var refreshToken = result.RefreshToken;
}
```

### Entity Relationships
```csharp
var universe = new Universe { Name = "Middle Earth", Description = "Tolkien's fantasy world" };
var character = new NotableFigure { Name = "Frodo", Description = "Hobbit from the Shire" };
var location = new Location { Name = "The Shire", Description = "Peaceful hobbit homeland" };

var relation = new EntityRelation(character, location, RelationTypeEnum.BornIn);
```

### Database Context
```csharp
// In startup configuration
services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://localhost:27017"));
services.AddScoped(sp => sp.GetService<IMongoClient>().GetDatabase("glimmer"));
services.AddScoped<GlimmerDbContext>();
```

## 🔄 Development Patterns

### Entity Conventions
1. All entities inherit from `BaseEntity`
2. Use `required` keyword for essential properties
3. Implement soft delete with `IsDeleted` flag
4. Use `Guid` for all primary keys
5. Follow enum naming: `{EntityType}TypeEnum`

### Service Patterns
1. Define interfaces for all services
2. Use async/await for all I/O operations
3. Return result objects for complex operations
4. Implement proper error handling and logging
5. Use dependency injection for all dependencies

### Security Patterns
1. Always hash passwords before storage
2. Use secure random generation for tokens
3. Implement proper token expiration
4. Log security-relevant events
5. Validate all inputs

## 🏗️ Future Enhancements

- [ ] Repository pattern implementation
- [ ] CQRS with MediatR
- [ ] Domain events and event handlers
- [ ] Specification pattern for queries
- [ ] Background services for cleanup tasks
- [ ] Audit logging infrastructure
- [ ] Multi-tenancy support
- [ ] Advanced caching strategies

## 📖 Additional Documentation

- [Authentication Service Documentation](Services/README_AUTHENTICATION.md)
- [Entity Relationship Guide](../docs/entity-relationships.md)
- [MongoDB Setup Guide](../docs/mongodb-setup.md)

---

**Note**: This is the domain layer of Glimmer. For the web application layer, see [Glimmer.Creator README](../Glimmer.Creator/README.md).