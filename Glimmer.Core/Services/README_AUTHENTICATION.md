# Glimmer Authentication Service

## Overview
The Authentication Service provides secure user authentication and authorization for the Glimmer Creator application using JWT (JSON Web Tokens) with refresh token support.

## Features

### ✅ User Registration & Login
- Secure password hashing using HMACSHA512
- Unique username and email validation
- Email verification support
- Account activation status

### ✅ JWT Token Management
- Access tokens (short-lived, default 60 minutes)
- Refresh tokens (long-lived, default 7 days)
- Token revocation and rotation
- Automatic token expiration handling

### ✅ Password Management
- Secure password hashing with salts
- Password change functionality
- Password reset with time-limited tokens
- Current password verification

### ✅ Security Features
- HMACSHA512 password hashing
- Cryptographically secure token generation
- Token rotation on refresh
- Revocation reason tracking
- Account activation/deactivation

## Architecture

### Models
- **User** - User account information
- **RefreshToken** - Refresh token management
- **PasswordResetToken** - Password reset token tracking
- **AuthenticationResult** - Authentication operation responses

### Service Interface (`IAuthenticationService`)

```csharp
Task<AuthenticationResult> RegisterAsync(string username, string email, string password);
Task<AuthenticationResult> LoginAsync(string usernameOrEmail, string password);
Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
Task<bool> RevokeTokenAsync(string refreshToken, string? reason = null);
Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
Task<string?> GeneratePasswordResetTokenAsync(string email);
Task<bool> ResetPasswordAsync(string token, string newPassword);
Task<bool> VerifyEmailAsync(string token);
Task<User?> GetUserByIdAsync(Guid userId);
Task<User?> GetUserByUsernameAsync(string username);
Task<User?> GetUserByEmailAsync(string email);
string HashPassword(string password, out string salt);
bool VerifyPassword(string password, string hash, string salt);
```

## Configuration

### appsettings.json
```json
{
  "Jwt": {
    "Secret": "YourSecretKey-MinLength32Characters-ChangeInProduction!",
    "Issuer": "Glimmer.Creator",
    "Audience": "Glimmer.Users",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Service Registration
```csharp
// In Program.cs
using Glimmer.Core.Extensions;

builder.Services.AddGlimmerCore();
```

## Usage Examples

### 1. User Registration
```csharp
var result = await _authService.RegisterAsync("johndoe", "john@example.com", "SecurePass123!");

if (result.Success)
{
    var accessToken = result.AccessToken;
    var refreshToken = result.RefreshToken;
    var user = result.User;
}
```

### 2. User Login
```csharp
var result = await _authService.LoginAsync("johndoe", "SecurePass123!");

if (result.Success)
{
    // Store tokens securely
    HttpContext.Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = result.ExpiresAt
    });
}
```

### 3. Token Refresh
```csharp
var refreshToken = HttpContext.Request.Cookies["refresh_token"];
var result = await _authService.RefreshTokenAsync(refreshToken);

if (result.Success)
{
    // Update tokens
}
```

### 4. Password Reset Flow
```csharp
// Step 1: Generate reset token
var token = await _authService.GeneratePasswordResetTokenAsync("user@example.com");
// Send token via email

// Step 2: Reset password
var success = await _authService.ResetPasswordAsync(token, "NewSecurePass123!");
```

### 5. Change Password
```csharp
var success = await _authService.ChangePasswordAsync(
    userId, 
    "OldPassword123!", 
    "NewSecurePass456!"
);
```

## Security Best Practices

### 1. JWT Secret
- Use a strong, randomly generated secret (minimum 32 characters)
- Store in environment variables, not in code
- Different secrets for dev/staging/production

### 2. Token Storage
- Store refresh tokens in HttpOnly cookies
- Never store tokens in localStorage (XSS vulnerability)
- Use Secure flag for cookies in production

### 3. Password Policy
- Implement minimum length requirements (8+ characters)
- Require complexity (uppercase, lowercase, numbers, symbols)
- Implement rate limiting on login attempts

### 4. Production Deployment
- Replace in-memory storage with MongoDB repositories
- Implement proper logging and monitoring
- Add IP-based rate limiting
- Enable HTTPS only
- Implement CORS policies

## Data Persistence

### Current Implementation
The service uses in-memory collections for development:
- `List<User> _users`
- `List<RefreshToken> _refreshTokens`
- `List<PasswordResetToken> _passwordResetTokens`

### Production Implementation
Replace with MongoDB repositories:

```csharp
public class AuthenticationService : IAuthenticationService
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<RefreshToken> _refreshTokens;
    private readonly IMongoCollection<PasswordResetToken> _passwordResetTokens;

    public AuthenticationService(IMongoDatabase database, IConfiguration configuration)
    {
        _users = database.GetCollection<User>("users");
        _refreshTokens = database.GetCollection<RefreshToken>("refreshTokens");
        _passwordResetTokens = database.GetCollection<PasswordResetToken>("passwordResetTokens");
        // ... initialization
    }
}
```

## Token Claims Structure

Access tokens include the following claims:
- `NameIdentifier` - User UUID
- `Name` - Username
- `Email` - User email
- `Jti` - Unique token ID

## Error Handling

All authentication operations return `AuthenticationResult` with:
- `Success` - Boolean indicating success/failure
- `Message` - Descriptive error or success message
- `User` - User object (on success)
- `AccessToken` - JWT access token (on success)
- `RefreshToken` - Refresh token string (on success)
- `ExpiresAt` - Token expiration time (on success)

## Testing

### Unit Test Example
```csharp
[Fact]
public async Task RegisterAsync_ValidData_ReturnsSuccess()
{
    // Arrange
    var config = new ConfigurationBuilder().Build();
    var service = new AuthenticationService(config);

    // Act
    var result = await service.RegisterAsync("testuser", "test@example.com", "Password123!");

    // Assert
    Assert.True(result.Success);
    Assert.NotNull(result.AccessToken);
    Assert.NotNull(result.User);
}
```

## Dependencies

- `System.IdentityModel.Tokens.Jwt` 8.0.0 - JWT token generation
- `Microsoft.IdentityModel.Tokens` 8.0.0 - Token validation
- `Microsoft.Extensions.Configuration.Abstractions` 8.0.0 - Configuration support
- `MongoDB.Driver` 2.22.0 - Database persistence (future)

## Future Enhancements

- [ ] Two-factor authentication (2FA)
- [ ] OAuth2 integration (Google, GitHub)
- [ ] Role-based access control (RBAC)
- [ ] Session management
- [ ] Audit logging
- [ ] Failed login attempt tracking
- [ ] Account lockout after failed attempts
- [ ] Email verification workflow
- [ ] Password strength meter
- [ ] Remember me functionality
