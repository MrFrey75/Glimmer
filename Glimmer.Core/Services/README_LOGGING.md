# Logging System Documentation

## Overview

Glimmer uses **Serilog** as its logging framework, providing structured logging with multiple output sinks (Console, File, and MongoDB).

## Architecture

### Components

1. **Serilog** - Main logging library
2. **Serilog.AspNetCore** - ASP.NET Core integration
3. **Serilog.Sinks.Console** - Console output
4. **Serilog.Sinks.File** - File-based logging with rotation
5. **Serilog.Sinks.MongoDB** - MongoDB logging for persistent storage
6. **Serilog.Enrichers** - Add contextual information (Machine Name, Thread ID, Environment)

### Log Levels

Serilog supports the following log levels (from lowest to highest severity):

- **Verbose** - Detailed diagnostic information
- **Debug** - Debugging information
- **Information** - Informational messages
- **Warning** - Warning messages
- **Error** - Error messages
- **Fatal** - Critical failures that require immediate attention

## Configuration

### appsettings.json

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Sinks.MongoDB" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/glimmer-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] [{ThreadId}] {Message:lj}{NewLine}{Exception}",
          "fileSizeLimitBytes": 10485760,
          "rollOnFileSizeLimit": true
        }
      },
      {
        "Name": "MongoDB",
        "Args": {
          "databaseUrl": "mongodb://localhost:27017/GlimmerDB",
          "collectionName": "Logs",
          "cappedMaxSizeMb": 256,
          "cappedMaxDocuments": 50000
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId", "WithEnvironmentName" ],
    "Properties": {
      "Application": "Glimmer.Creator"
    }
  }
}
```

### appsettings.Development.json

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "Microsoft.AspNetCore": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## Log Storage

### File Logs

- **Location**: `Logs/` directory in application root
- **Format**: `glimmer-YYYYMMDD.log`
- **Rotation**: Daily
- **Retention**: 30 days
- **Size Limit**: 10 MB per file (rolls over to new file when exceeded)

### MongoDB Logs

- **Collection**: `Logs` in GlimmerDB
- **Type**: Capped collection
- **Max Size**: 256 MB
- **Max Documents**: 50,000
- **Structure**: JSON documents with enriched properties

Example log document:
```json
{
  "_id": ObjectId("..."),
  "Timestamp": ISODate("2024-01-15T10:30:00.123Z"),
  "Level": "Information",
  "MessageTemplate": "User {Username} (ID: {UserId}) logged in successfully",
  "Message": "User john_doe (ID: 123e4567-e89b-12d3-a456-426614174000) logged in successfully",
  "Properties": {
    "Username": "john_doe",
    "UserId": "123e4567-e89b-12d3-a456-426614174000",
    "SourceContext": "Glimmer.Creator.Controllers.AccountController",
    "MachineName": "SERVER01",
    "ThreadId": 12,
    "Application": "Glimmer.Creator",
    "EnvironmentName": "Development"
  },
  "RenderedMessage": "User john_doe (ID: 123e4567-e89b-12d3-a456-426614174000) logged in successfully"
}
```

### Console Logs

Console logs are formatted for easy reading during development:

```
[2024-01-15 10:30:00.123 +00:00] [INF] [Glimmer.Creator.Controllers.AccountController] User john_doe (ID: 123e4567-e89b-12d3-a456-426614174000) logged in successfully
```

## Usage in Code

### Structured Logging (Recommended)

```csharp
// Good - Structured logging with named properties
_logger.LogInformation("User {Username} (ID: {UserId}) logged in successfully", 
    username, userId);

// Good - Structured logging with multiple properties
_logger.LogWarning("Failed login attempt for user: {UsernameOrEmail}. Reason: {Reason}", 
    usernameOrEmail, failureReason);

// Good - Error with exception
_logger.LogError(ex, "Error during registration for username: {Username}, email: {Email}", 
    username, email);
```

### String Interpolation (Avoid)

```csharp
// Bad - Loses structured data
_logger.LogInformation($"User {username} logged in successfully");

// Bad - No context for querying
_logger.LogError($"Error: {ex.Message}");
```

### Log Level Guidelines

#### Debug
```csharp
_logger.LogDebug("Checking if superuser exists");
_logger.LogDebug("Query returned {Count} results", resultCount);
```

#### Information
```csharp
_logger.LogInformation("User {Username} (ID: {UserId}) registered successfully", username, userId);
_logger.LogInformation("Application started successfully");
_logger.LogInformation("Processing {ItemCount} items", items.Count);
```

#### Warning
```csharp
_logger.LogWarning("Failed login attempt for user: {Username}", username);
_logger.LogWarning("Database connection slow: {Duration}ms", duration);
_logger.LogWarning("Cache miss for key: {CacheKey}", key);
```

#### Error
```csharp
_logger.LogError(ex, "Error during password reset for email: {Email}", email);
_logger.LogError(ex, "Failed to save entity {EntityId} to database", entityId);
```

#### Fatal
```csharp
_logger.LogCritical(ex, "Database connection lost. Application shutting down");
_logger.LogCritical("Configuration error: Missing required setting {SettingName}", settingName);
```

## Request Logging

HTTP requests are automatically logged with:
- HTTP Method
- Request Path
- Status Code
- Duration
- User Agent
- Authenticated Username (if logged in)

Example:
```
[2024-01-15 10:30:00.123] [INF] HTTP GET /Account/Login responded 200 in 45.2341 ms
```

## Querying Logs

### MongoDB Queries

#### Find all errors in the last hour
```javascript
db.Logs.find({
  Level: "Error",
  Timestamp: { $gte: new Date(Date.now() - 3600000) }
}).sort({ Timestamp: -1 })
```

#### Find logs for specific user
```javascript
db.Logs.find({
  "Properties.Username": "john_doe"
}).sort({ Timestamp: -1 })
```

#### Find slow requests (>1000ms)
```javascript
db.Logs.find({
  MessageTemplate: /responded/,
  Message: /\d{4,}\.\d+ ms/
}).sort({ Timestamp: -1 })
```

#### Count errors by source
```javascript
db.Logs.aggregate([
  { $match: { Level: "Error" } },
  { $group: { _id: "$Properties.SourceContext", count: { $sum: 1 } } },
  { $sort: { count: -1 } }
])
```

### File Logs

```bash
# View today's log
tail -f Logs/glimmer-$(date +%Y%m%d).log

# Search for errors
grep "ERR" Logs/glimmer-*.log

# Search for specific user
grep "john_doe" Logs/glimmer-*.log

# Count log levels
grep -oh "\[INF\]\|\[WRN\]\|\[ERR\]" Logs/glimmer-*.log | sort | uniq -c
```

## Best Practices

### ✅ DO

1. **Use structured logging** with named properties
   ```csharp
   _logger.LogInformation("Order {OrderId} created by {UserId}", orderId, userId);
   ```

2. **Include context** in error logs
   ```csharp
   _logger.LogError(ex, "Failed to process order {OrderId} for user {UserId}", orderId, userId);
   ```

3. **Log at entry/exit** of critical operations
   ```csharp
   _logger.LogDebug("Starting universe creation for user {UserId}", userId);
   // ... operation ...
   _logger.LogInformation("Universe {UniverseId} created successfully", universe.Uuid);
   ```

4. **Use appropriate log levels**
   - Debug: Development/diagnostic info
   - Information: Normal operations
   - Warning: Unexpected but handled situations
   - Error: Errors that need attention
   - Fatal: Application-breaking errors

5. **Sanitize sensitive data**
   ```csharp
   // Never log passwords, tokens, or sensitive PII
   _logger.LogInformation("Password changed for user {UserId}", userId); // Good
   // _logger.LogDebug("Password: {Password}", password); // NEVER DO THIS
   ```

### ❌ DON'T

1. **Don't use string interpolation**
   ```csharp
   _logger.LogInformation($"User {userId} created"); // Bad - not structured
   ```

2. **Don't log excessive data**
   ```csharp
   _logger.LogDebug("Entity: {Entity}", JsonSerializer.Serialize(hugeObject)); // Bad
   ```

3. **Don't log in tight loops without throttling**
   ```csharp
   foreach (var item in millionItems)
   {
       _logger.LogDebug("Processing {Item}", item); // Bad - too many logs
   }
   ```

4. **Don't log sensitive information**
   - Passwords
   - Access tokens
   - Credit card numbers
   - Social security numbers
   - Email content

## Performance Considerations

1. **Log Level Filtering**: Production environments should use `Information` or higher to reduce volume
2. **Async Writes**: Serilog writes asynchronously to avoid blocking
3. **Batching**: MongoDB sink batches writes for better performance
4. **Capped Collections**: MongoDB logs are capped to prevent unlimited growth
5. **File Rotation**: Automatic daily rotation prevents files from becoming too large

## Monitoring and Alerts

Consider setting up alerts for:

1. **Error Rate**: Alert if error count exceeds threshold in time window
2. **Failed Logins**: Multiple failed login attempts from same IP
3. **Slow Requests**: Requests taking longer than expected
4. **Critical Errors**: Any fatal-level logs
5. **Disk Space**: Monitor log file directory size

## Troubleshooting

### Logs not appearing in MongoDB

1. Check MongoDB connection string in appsettings.json
2. Verify MongoDB is running: `mongosh --eval "db.version()"`
3. Check MongoDB logs: `docker logs mongodb` (if using Docker)
4. Verify collection exists: `db.Logs.count()`

### File logs not being created

1. Check write permissions on `Logs/` directory
2. Verify path in appsettings.json
3. Check application startup logs in console

### Too many logs

1. Increase minimum log level in appsettings.json
2. Add more override rules for noisy components
3. Reduce retention period for file logs
4. Decrease MongoDB capped collection size

## Example Implementations

### Controller Logging
```csharp
public class UniverseController : Controller
{
    private readonly IEntityService _entityService;
    private readonly ILogger<UniverseController> _logger;

    public UniverseController(IEntityService entityService, ILogger<UniverseController> logger)
    {
        _entityService = entityService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUniverseViewModel model)
    {
        _logger.LogInformation("Creating universe with name: {Name}", model.Name);
        
        try
        {
            var universe = await _entityService.CreateUniverseAsync(model.Name, model.Description);
            
            _logger.LogInformation("Universe {UniverseId} created successfully by user {UserId}", 
                universe.Uuid, GetCurrentUserId());
            
            return RedirectToAction("Details", new { id = universe.Uuid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create universe with name: {Name}", model.Name);
            ModelState.AddModelError("", "Failed to create universe");
            return View(model);
        }
    }
}
```

### Service Logging
```csharp
public class EntityService : IEntityService
{
    private readonly IUniverseRepository _universeRepository;
    private readonly ILogger<EntityService> _logger;

    public async Task<Universe> CreateUniverseAsync(string name, string description)
    {
        _logger.LogDebug("Creating universe: {Name}", name);
        
        var universe = new Universe
        {
            Name = name,
            Description = description
        };

        try
        {
            await _universeRepository.CreateAsync(universe);
            _logger.LogInformation("Universe {UniverseId} created: {Name}", universe.Uuid, name);
            return universe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create universe: {Name}", name);
            throw;
        }
    }
}
```

## Related Documentation

- [Serilog Official Documentation](https://serilog.net/)
- [Serilog Best Practices](https://github.com/serilog/serilog/wiki/Best-Practices)
- [ASP.NET Core Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)

## Summary

The Glimmer logging system provides:
- ✅ Structured logging for easy querying and analysis
- ✅ Multiple output targets (Console, File, MongoDB)
- ✅ Automatic request logging
- ✅ Rich contextual information (machine name, thread ID, etc.)
- ✅ Configurable log levels per component
- ✅ Log rotation and retention management
- ✅ Production-ready performance
