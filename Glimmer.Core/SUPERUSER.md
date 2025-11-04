# Glimmer Superuser Documentation

## Overview
The Glimmer authentication system includes a built-in **superuser** account that is automatically seeded when the `AuthenticationService` is initialized. This superuser cannot be deleted or deactivated, ensuring there is always an administrative account available.

## Superuser Credentials

### Default Login
```
Username: Admin
Password: Password1234
Email: admin@glimmer.local
```

⚠️ **IMPORTANT**: Change the password immediately after first login in production environments!

## Superuser Properties

The superuser account has the following characteristics:

### Fixed Properties
- **UUID**: `00000000-0000-0000-0000-000000000001` (fixed)
- **Username**: `Admin`
- **Name**: `Administrator`
- **Description**: `System Administrator - Cannot be deleted`
- **Email**: `admin@glimmer.local`
- **IsSuperUser**: `true`
- **IsActive**: `true`
- **EmailVerified**: `true`

### Protection Features
✅ **Cannot be deleted** - `DeleteUserAsync()` will return false
✅ **Cannot be deactivated** - `DeactivateUserAsync()` will return false
✅ **Always active** - Account remains active permanently
✅ **Email verified** - No verification needed
✅ **Persistent** - Automatically recreated if missing

## Seeding Mechanism

The superuser is automatically seeded when the `AuthenticationService` is instantiated:

```csharp
public AuthenticationService(IConfiguration configuration)
{
    // ... configuration setup
    
    // Seed superuser on initialization
    SeedSuperUser();
}

private void SeedSuperUser()
{
    // Check if superuser already exists
    if (_users.Any(u => u.Username == "Admin" && u.IsSuperUser))
    {
        return;
    }

    // Create superuser with hashed password
    var passwordHash = HashPassword("Password1234", out string salt);
    
    var superUser = new User
    {
        Uuid = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Username = "Admin",
        // ... other properties
        IsSuperUser = true
    };

    _users.Add(superUser);
}
```

## Usage Examples

### Login as Superuser
```csharp
var authService = serviceProvider.GetRequiredService<IAuthenticationService>();
var result = await authService.LoginAsync("Admin", "Password1234");

if (result.Success)
{
    Console.WriteLine($"Logged in as: {result.User.Username}");
    Console.WriteLine($"Is SuperUser: {result.User.IsSuperUser}");
    // Access token includes "SuperUser" role claim
}
```

### Check if User is Superuser
```csharp
var user = await authService.GetUserByUsernameAsync("Admin");
if (user != null && user.IsSuperUser)
{
    Console.WriteLine("This is a superuser account");
}
```

### Attempt to Delete Superuser (Will Fail)
```csharp
var deleted = await authService.DeleteUserAsync(adminUserId);
// Returns false - superuser cannot be deleted
```

### JWT Token Claims
When the superuser logs in, the JWT token includes an additional role claim:

```csharp
Claims:
- NameIdentifier: 00000000-0000-0000-0000-000000000001
- Name: Admin
- Email: admin@glimmer.local
- Role: SuperUser (only for superuser)
- Jti: <unique token id>
```

## User Model Changes

The `User` model has been extended with a `IsSuperUser` property:

```csharp
public class User : BaseEntity
{
    // ... existing properties
    public bool IsSuperUser { get; set; } = false;
}
```

## New Service Methods

### DeleteUserAsync
```csharp
Task<bool> DeleteUserAsync(Guid userId);
```
- Deletes a user account permanently
- Returns `false` if user is a superuser
- Removes user data and associated tokens

### DeactivateUserAsync
```csharp
Task<bool> DeactivateUserAsync(Guid userId);
```
- Deactivates a user account (soft delete)
- Returns `false` if user is a superuser
- Sets `IsActive = false` and revokes all tokens

## Security Considerations

### ⚠️ Production Deployment

1. **Change Default Password**
   ```csharp
   await authService.ChangePasswordAsync(
       adminUserId, 
       "Password1234", 
       "NewStrongPassword!"
   );
   ```

2. **Use Environment Variables**
   - Store credentials in secure configuration
   - Never hardcode production passwords

3. **Monitor Superuser Activity**
   - Log all superuser actions
   - Implement audit trails
   - Set up alerts for superuser logins

4. **Rotate Password Regularly**
   - Change superuser password periodically
   - Use strong, unique passwords

### 🔒 Best Practices

- **Limit Superuser Usage**: Only use for administrative tasks
- **Create Admin Users**: Create regular admin users for daily operations
- **Enable 2FA**: Implement two-factor authentication for superuser
- **IP Whitelisting**: Restrict superuser access to known IPs
- **Session Timeout**: Use shorter session timeouts for superuser

## Testing

A test suite is provided to verify superuser functionality:

```bash
# Location: Glimmer.Core/Tests/SuperUserTest.cs

Tests Included:
1. ✅ Login with correct credentials
2. ✅ Login rejection with wrong password
3. ✅ User lookup by username
4. ✅ Deletion protection
5. ✅ Deactivation protection
6. ✅ Normal user deletion comparison
```

## Migration from In-Memory to MongoDB

When migrating to MongoDB persistence:

1. **Seed on First Run**
   ```csharp
   public async Task SeedSuperUserAsync()
   {
       var existing = await _userCollection
           .Find(u => u.Username == "Admin" && u.IsSuperUser)
           .FirstOrDefaultAsync();
       
       if (existing == null)
       {
           // Create and insert superuser
       }
   }
   ```

2. **Add Index on IsSuperUser**
   ```csharp
   await _userCollection.Indexes.CreateOneAsync(
       new CreateIndexModel<User>(
           Builders<User>.IndexKeys.Ascending(u => u.IsSuperUser)
       )
   );
   ```

3. **Database Seed Script**
   ```javascript
   // MongoDB seed script
   db.users.insertOne({
       _id: UUID("00000000-0000-0000-0000-000000000001"),
       username: "Admin",
       isSuperUser: true,
       // ... other fields
   });
   ```

## FAQ

### Q: Can I change the superuser username?
**A**: Yes, but you'll need to modify the `SeedSuperUser()` method in `AuthenticationService.cs`.

### Q: Can I have multiple superusers?
**A**: Yes, create additional users and set `IsSuperUser = true`. They will have the same protections.

### Q: What happens if I delete the superuser from the database directly?
**A**: The superuser will be automatically recreated the next time `AuthenticationService` is initialized.

### Q: Can the superuser password be reset via the forgot password flow?
**A**: Yes, but it's recommended to implement additional verification for superuser password resets.

### Q: How do I change the default password in code?
**A**: Modify the password in `SeedSuperUser()` method:
```csharp
var passwordHash = HashPassword("YourNewPassword", out string salt);
```

## Related Files

- `Glimmer.Core/Models/User.cs` - User model with IsSuperUser property
- `Glimmer.Core/Services/AuthenticationService.cs` - Seeding logic
- `Glimmer.Core/Tests/SuperUserTest.cs` - Test suite
- `Glimmer.Creator/Controllers/AccountController.cs` - Login endpoints

## Changelog

### Version 1.0 (2025-11-04)
- ✅ Added superuser seeding on service initialization
- ✅ Added `IsSuperUser` property to User model
- ✅ Implemented deletion protection
- ✅ Implemented deactivation protection
- ✅ Added "SuperUser" role claim to JWT tokens
- ✅ Created test suite for verification
- ✅ Added `DeleteUserAsync()` method
- ✅ Added `DeactivateUserAsync()` method
