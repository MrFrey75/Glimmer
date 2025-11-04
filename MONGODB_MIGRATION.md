# MongoDB Migration Status

## Completed ✅

### 1. MongoDB Repositories Created
- ✅ `UserRepository.cs` - CRUD operations for users with unique indexes on Username and Email
- ✅ `TokenRepository.cs` - Manages RefreshTokens and PasswordResetTokens
- ✅ `UniverseRepository.cs` - CRUD operations for universes
- ✅ `RelationRepository.cs` - Manages EntityRelations with UniverseId support

### 2. Configuration
- ✅ `MongoDbSettings.cs` - MongoDB connection settings
- ✅ Updated `ServiceCollectionExtensions.cs` - Registers MongoDB client, database, and all repositories
- ✅ Updated `Program.cs` - Initializes MongoDB and seeds superuser on startup

### 3. Model Updates
- ✅ Added `UniverseId` property to `EntityRelation` model
- ✅ Updated `EntityRelation` constructor to accept `universeId` parameter

### 4. Database Indexes
All repositories automatically create indexes on initialization:
- Users: Unique indexes on Username and Email
- RefreshTokens: Unique index on Token, index on UserId
- PasswordResetTokens: Unique index on Token
- Universes: Unique index on Uuid, index on CreatedBy.Uuid
- Relations: Unique index on Oid, indexes on FromEntity.Uuid and ToEntity.Uuid

## Remaining Work ⏳

### AuthenticationService.cs
**Status**: Still uses in-memory `List<>` collections

**Required Changes**:
```csharp
// OLD (In-Memory):
private readonly List<User> _users = new();
private readonly List<RefreshToken> _refreshTokens = new();
private readonly List<PasswordResetToken> _passwordResetTokens = new();

// NEW (MongoDB):
private readonly IUserRepository _userRepository;
private readonly ITokenRepository _tokenRepository;

public AuthenticationService(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IConfiguration configuration)
{
    _userRepository = userRepository;
    _tokenRepository = tokenRepository;
    // ... rest of initialization
}
```

**Methods Needing Updates** (13 methods):
1. `RegisterAsync` - Use `_userRepository.CreateAsync(user)`
2. `LoginAsync` - Use `_userRepository.GetByUsernameAsync()` or `GetByEmailAsync()`
3. `RefreshTokenAsync` - Use `_tokenRepository.GetRefreshTokenAsync()` and `UpdateRefreshTokenAsync()`
4. `RevokeTokenAsync` - Use `_tokenRepository.GetRefreshTokenAsync()` and `UpdateRefreshTokenAsync()`
5. `ChangePasswordAsync` - Use `_userRepository.GetByIdAsync()` and `UpdateAsync()`
6. `GeneratePasswordResetTokenAsync` - Use `_userRepository.GetByEmailAsync()` and `_tokenRepository.CreatePasswordResetTokenAsync()`
7. `ResetPasswordAsync` - Use `_tokenRepository.GetPasswordResetTokenAsync()` and `_userRepository.UpdateAsync()`
8. `VerifyEmailAsync` - Use `_userRepository.GetByIdAsync()` and `UpdateAsync()`
9. `GetUserByIdAsync` - Use `_userRepository.GetByIdAsync()`
10. `GetUserByUsernameAsync` - Use `_userRepository.GetByUsernameAsync()`
11. `GetUserByEmailAsync` - Use `_userRepository.GetByEmailAsync()`
12. `DeleteUserAsync` - Use `_userRepository.DeleteAsync()`
13. `DeactivateUserAsync` - Use `_userRepository.GetByIdAsync()` and `UpdateAsync()`

**Special Method**:
- `SeedSuperUser()` → Rename to `EnsureSuperUserExistsAsync()` and make async
- Already updated in interface and called from Program.cs

### EntityService.cs
**Status**: Still uses in-memory `List<Universe>` and `Dictionary<Guid, List<EntityRelation>>`

**Required Changes**:
```csharp
// OLD (In-Memory):
private readonly List<Universe> _universes = new();
private readonly Dictionary<Guid, List<EntityRelation>> _relations = new();
private int _nextRelationId = 1;

// NEW (MongoDB):
private readonly IUniverseRepository _universeRepository;
private readonly IRelationRepository _relationRepository;

public EntityService(
    IUniverseRepository universeRepository,
    IRelationRepository relationRepository)
{
    _universeRepository = universeRepository;
    _relationRepository = relationRepository;
}
```

**Methods Needing Updates** (70+ methods):

**Universe Operations** (6 methods):
1. `CreateUniverseAsync` - Use `_universeRepository.CreateAsync()`
2. `GetUniverseByIdAsync` - Use `_universeRepository.GetByIdAsync()`
3. `GetUniversesByUserAsync` - Use `_universeRepository.GetByUserIdAsync()`
4. `GetAllUniversesAsync` - Use `_universeRepository.GetAllAsync()`
5. `UpdateUniverseAsync` - Use `_universeRepository.UpdateAsync()`
6. `DeleteUniverseAsync` - Use `_universeRepository.DeleteAsync()`

**Entity Operations** (Artifact, CannonEvent, Faction, Location, NotableFigure, Fact - 5 methods each = 30 methods):
- All Create methods: Get universe from repository, add entity, save universe back
- All Get methods: Get universe from repository, filter entities
- All Update methods: Get universe from repository, update entity, save universe back
- All Delete methods: Get universe from repository, mark entity as deleted, save universe back

**Relation Operations** (6 methods):
1. `CreateRelationAsync` - Use `_relationRepository.CreateAsync()` with `GetNextIdAsync()`
2. `GetRelationByIdAsync` - Use `_relationRepository.GetByIdAsync()`
3. `GetRelationsByUniverseAsync` - Use `_relationRepository.GetByUniverseIdAsync()`
4. `GetRelationsByEntityAsync` - Use `_relationRepository.GetByEntityIdAsync()`
5. `UpdateRelationAsync` - Use `_relationRepository.UpdateAsync()`
6. `DeleteRelationAsync` - Use `_relationRepository.DeleteAsync()`

**Generic Operations** (3 methods):
- These remain largely unchanged as they operate on Universe entities

## Migration Pattern Examples

### Pattern 1: Simple Repository Call
```csharp
// Before:
public async Task<User?> GetUserByIdAsync(Guid userId)
{
    await Task.CompletedTask;
    return _users.FirstOrDefault(u => u.Uuid == userId);
}

// After:
public async Task<User?> GetUserByIdAsync(Guid userId)
{
    return await _userRepository.GetByIdAsync(userId);
}
```

### Pattern 2: Create with Repository
```csharp
// Before:
var user = new User { /* ... */ };
_users.Add(user);

// After:
var user = new User { /* ... */ };
await _userRepository.CreateAsync(user);
```

### Pattern 3: Update with Repository
```csharp
// Before:
var existing = _users.FirstOrDefault(u => u.Uuid == userId);
if (existing == null) return false;
existing.Property = newValue;
existing.UpdatedAt = DateTime.UtcNow;
return true;

// After:
var existing = await _userRepository.GetByIdAsync(userId);
if (existing == null) return false;
existing.Property = newValue;
existing.UpdatedAt = DateTime.UtcNow;
return await _userRepository.UpdateAsync(existing);
```

### Pattern 4: Universe Entity Operations
```csharp
// Before:
var universe = _universes.FirstOrDefault(u => u.Uuid == universeId);
if (universe == null) return null;
var entity = new Artifact { /* ... */ };
universe.Artifacts.Add(entity);
universe.UpdatedAt = DateTime.UtcNow;
return entity;

// After:
var universe = await _universeRepository.GetByIdAsync(universeId);
if (universe == null) return null;
var entity = new Artifact { /* ... */ };
universe.Artifacts.Add(entity);
universe.UpdatedAt = DateTime.UtcNow;
await _universeRepository.UpdateAsync(universe);
return entity;
```

## Testing MongoDB Connection

Before running, ensure MongoDB is running:

```bash
# Using Docker:
docker run -d -p 27017:27017 --name mongodb mongo:latest

# Or install MongoDB locally
# Ubuntu/Debian:
sudo apt-get install mongodb

# macOS:
brew install mongodb-community

# Start MongoDB:
sudo systemctl start mongodb  # Linux
brew services start mongodb-community  # macOS
```

## Connection String
Default in `appsettings.json`:
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "GlimmerDB"
  }
}
```

## Current State
- ✅ MongoDB infrastructure ready
- ✅ Repositories implemented and registered
- ✅ Configuration updated
- ⏳ Services need refactoring (AuthenticationService, EntityService)
- ✅ Program.cs updated to seed superuser on startup

## Next Steps

1. **Install/Start MongoDB** (if not already running)
2. **Update AuthenticationService** to use repositories
3. **Update EntityService** to use repositories
4. **Test the application**
5. **Remove backup files** (*.bak)
6. **Commit changes**

## Automated Migration Script

A script could be created to automate the refactoring:
- Replace `_users.` with `await _userRepository.`
- Replace LINQ operations with repository method calls
- Add `await` keywords where needed
- Update method signatures to be async if not already

However, manual review is recommended to ensure correctness.
