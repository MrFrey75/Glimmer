using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Glimmer.Core.Models;
using Glimmer.Core.Repositories;
using Glimmer.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Glimmer.Core.Services;

public interface IAuthenticationService
{
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
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> DeactivateUserAsync(Guid userId);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    public AuthenticationService(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IConfiguration configuration,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _configuration = configuration;
        _logger = logger;
        _jwtSecret = configuration["Jwt:Secret"] ?? "GlimmerSecretKey-ChangeInProduction-MinLength32Characters!";
        _jwtIssuer = configuration["Jwt:Issuer"] ?? "Glimmer.Creator";
        _jwtAudience = configuration["Jwt:Audience"] ?? "Glimmer.Users";
        _accessTokenExpirationMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
        _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
    }

    public async Task<AuthenticationResult> RegisterAsync(string username, string email, string password)
    {
        _logger.LogInformation("Registration attempt for username: {Username}, email: {Email}", username, email);
        
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Registration failed: Missing required fields for username: {Username}", username);
            return new AuthenticationResult { Success = false, Message = "Username, email, and password are required." };
        }

        if (await _userRepository.GetByUsernameAsync(username) != null)
        {
            _logger.LogWarning("Registration failed: Username {Username} already exists", username);
            return new AuthenticationResult { Success = false, Message = "Username already exists." };
        }

        if (await _userRepository.GetByEmailAsync(email) != null)
        {
            _logger.LogWarning("Registration failed: Email {Email} already exists", email);
            return new AuthenticationResult { Success = false, Message = "Email already exists." };
        }

        var passwordHash = HashPassword(password, out string salt);

        var user = new User
        {
            Uuid = Guid.NewGuid(),
            Name = username,
            Description = $"User account for {username}",
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            IsActive = true,
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);
        _logger.LogInformation("User {Username} (ID: {UserId}) registered successfully", user.Username, user.Uuid);

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user.Uuid);

        return new AuthenticationResult
        {
            Success = true,
            Message = "Registration successful.",
            User = user,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes)
        };
    }

    public async Task<AuthenticationResult> LoginAsync(string usernameOrEmail, string password)
    {
        _logger.LogDebug("Login attempt for: {UsernameOrEmail}", usernameOrEmail);
        
        var user = await _userRepository.GetByUsernameAsync(usernameOrEmail);
        if (user == null)
        {
            user = await _userRepository.GetByEmailAsync(usernameOrEmail);
        }

        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found for: {UsernameOrEmail}", usernameOrEmail);
            return new AuthenticationResult { Success = false, Message = "Invalid username/email or password." };
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed: Account inactive for user {Username} (ID: {UserId})", user.Username, user.Uuid);
            return new AuthenticationResult { Success = false, Message = "Account is inactive." };
        }

        if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogWarning("Login failed: Invalid password for user {Username} (ID: {UserId})", user.Username, user.Uuid);
            return new AuthenticationResult { Success = false, Message = "Invalid username/email or password." };
        }

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("User {Username} (ID: {UserId}) logged in successfully", user.Username, user.Uuid);

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user.Uuid);

        return new AuthenticationResult
        {
            Success = true,
            Message = "Login successful.",
            User = user,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes)
        };
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
    {
        _logger.LogDebug("Token refresh attempt");
        
        var token = await _tokenRepository.GetRefreshTokenAsync(refreshToken);

        if (token == null || !token.IsActive)
        {
            _logger.LogWarning("Token refresh failed: Invalid or expired refresh token");
            return new AuthenticationResult { Success = false, Message = "Invalid or expired refresh token." };
        }

        var user = await _userRepository.GetByIdAsync(token.UserId);

        if (user == null || !user.IsActive)
        {
            _logger.LogWarning("Token refresh failed: User {UserId} not found or inactive", token.UserId);
            return new AuthenticationResult { Success = false, Message = "User not found or inactive." };
        }

        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = await GenerateRefreshTokenAsync(user.Uuid);

        token.RevokedAt = DateTime.UtcNow;
        token.RevokedReason = "Replaced by new token";
        token.ReplacedByToken = newRefreshToken.Token;
        await _tokenRepository.UpdateRefreshTokenAsync(token);

        _logger.LogInformation("Token refreshed successfully for user {Username} (ID: {UserId})", user.Username, user.Uuid);

        return new AuthenticationResult
        {
            Success = true,
            Message = "Token refreshed successfully.",
            User = user,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes)
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, string? reason = null)
    {
        _logger.LogDebug("Token revocation attempt");
        
        var token = await _tokenRepository.GetRefreshTokenAsync(refreshToken);

        if (token == null || token.IsRevoked)
        {
            _logger.LogWarning("Token revocation failed: Token not found or already revoked");
            return false;
        }

        token.RevokedAt = DateTime.UtcNow;
        token.RevokedReason = reason ?? "Manually revoked";
        await _tokenRepository.UpdateRefreshTokenAsync(token);

        _logger.LogInformation("Token revoked for user ID: {UserId}, reason: {Reason}", token.UserId, token.RevokedReason);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        _logger.LogInformation("Password change attempt for user ID: {UserId}", userId);

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("Password change failed: User {UserId} not found", userId);
            return false;
        }

        if (!VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogWarning("Password change failed for user {Username} (ID: {UserId}): Invalid current password", user.Username, userId);
            return false;
        }

        var newPasswordHash = HashPassword(newPassword, out string salt);
        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = salt;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(userId);

        _logger.LogInformation("Password changed successfully for user {Username} (ID: {UserId})", user.Username, userId);
        return true;
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(string email)
    {
        _logger.LogInformation("Password reset token requested for email: {Email}", email);
        
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            _logger.LogWarning("Password reset failed: Email {Email} not found", email);
            return null;
        }

        var token = new PasswordResetToken
        {
            UserId = user.Uuid,
            Token = GenerateSecureToken(),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            CreatedAt = DateTime.UtcNow
        };

        await _tokenRepository.CreatePasswordResetTokenAsync(token);

        _logger.LogInformation("Password reset token generated for user {Username} (ID: {UserId})", user.Username, user.Uuid);
        return token.Token;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        _logger.LogDebug("Password reset attempt with token");
        
        var resetToken = await _tokenRepository.GetPasswordResetTokenAsync(token);

        if (resetToken == null || !resetToken.IsValid)
        {
            _logger.LogWarning("Password reset failed: Invalid or expired token");
            return false;
        }

        var user = await _userRepository.GetByIdAsync(resetToken.UserId);

        if (user == null)
        {
            _logger.LogWarning("Password reset failed: User {UserId} not found", resetToken.UserId);
            return false;
        }

        var newPasswordHash = HashPassword(newPassword, out string salt);
        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = salt;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        resetToken.IsUsed = true;
        resetToken.UsedAt = DateTime.UtcNow;
        await _tokenRepository.UpdatePasswordResetTokenAsync(resetToken);

        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(user.Uuid);

        _logger.LogInformation("Password reset successfully for user {Username} (ID: {UserId})", user.Username, user.Uuid);
        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        _logger.LogDebug("Email verification attempt with token");
        
        // Token here is the user's UUID for email verification
        if (!Guid.TryParse(token, out Guid userId))
        {
            _logger.LogWarning("Email verification failed: Invalid token format");
            return false;
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("Email verification failed: User {UserId} not found", userId);
            return false;
        }

        user.EmailVerified = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Email verified for user {Username} (ID: {UserId})", user.Username, userId);
        return true;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        _logger.LogInformation("User deletion attempt for user ID: {UserId}", userId);

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("User deletion failed: User {UserId} not found", userId);
            return false;
        }

        // Prevent deletion of superuser
        if (user.IsSuperUser)
        {
            _logger.LogWarning("Attempted to delete superuser {Username} (ID: {UserId}) - operation denied", user.Username, userId);
            return false;
        }

        await _userRepository.DeleteAsync(userId);
        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(userId);
        // Note: Password reset tokens will be handled by MongoDB TTL indexes or cleanup jobs

        _logger.LogInformation("User {Username} (ID: {UserId}) deleted successfully", user.Username, userId);
        return true;
    }

    public async Task<bool> DeactivateUserAsync(Guid userId)
    {
        _logger.LogInformation("User deactivation attempt for user ID: {UserId}", userId);

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("User deactivation failed: User {UserId} not found", userId);
            return false;
        }

        // Prevent deactivation of superuser
        if (user.IsSuperUser)
        {
            _logger.LogWarning("Attempted to deactivate superuser {Username} (ID: {UserId}) - operation denied", user.Username, userId);
            return false;
        }

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Revoke all refresh tokens
        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(userId);

        _logger.LogInformation("User {Username} (ID: {UserId}) deactivated successfully", user.Username, userId);
        return true;
    }

    public string HashPassword(string password, out string salt)
    {
        var (hash, generatedSalt) = SecurityUtils.HashPassword(password);
        salt = generatedSalt;
        return hash;
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        return SecurityUtils.VerifyPassword(password, hash, salt);
    }

    private string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Uuid.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add superuser claim
        if (user.IsSuperUser)
        {
            claims.Add(new Claim(ClaimTypes.Role, "SuperUser"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId)
    {
        var token = new RefreshToken
        {
            UserId = userId,
            Token = GenerateSecureToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow
        };

        await _tokenRepository.CreateRefreshTokenAsync(token);
        return token;
    }

    private string GenerateSecureToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}