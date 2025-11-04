using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Glimmer.Core.Models;
using Glimmer.Core.Repositories;
using Microsoft.Extensions.Configuration;
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
    Task EnsureSuperUserExistsAsync();
    string HashPassword(string password, out string salt);
    bool VerifyPassword(string password, string hash, string salt);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IConfiguration _configuration;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    public AuthenticationService(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _configuration = configuration;
        _jwtSecret = configuration["Jwt:Secret"] ?? "GlimmerSecretKey-ChangeInProduction-MinLength32Characters!";
        _jwtIssuer = configuration["Jwt:Issuer"] ?? "Glimmer.Creator";
        _jwtAudience = configuration["Jwt:Audience"] ?? "Glimmer.Users";
        _accessTokenExpirationMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
        _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
    }

    public async Task EnsureSuperUserExistsAsync()
    {
        // Check if superuser already exists
        var existingSuperUser = await _userRepository.GetByUsernameAsync("Admin");
        if (existingSuperUser != null && existingSuperUser.IsSuperUser)
        {
            return;
        }

        var passwordHash = HashPassword("Password1234", out string salt);

        var superUser = new User
        {
            Uuid = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name = "Administrator",
            Description = "System Administrator - Cannot be deleted",
            Username = "Admin",
            Email = "admin@glimmer.local",
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            IsActive = true,
            EmailVerified = true,
            IsSuperUser = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(superUser);
    }

    public async Task<AuthenticationResult> RegisterAsync(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return new AuthenticationResult { Success = false, Message = "Username, email, and password are required." };
        }

        if (await _userRepository.GetByUsernameAsync(username) != null)
        {
            return new AuthenticationResult { Success = false, Message = "Username already exists." };
        }

        if (await _userRepository.GetByEmailAsync(email) != null)
        {
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
        var user = await _userRepository.GetByUsernameAsync(usernameOrEmail);
        if (user == null)
        {
            user = await _userRepository.GetByEmailAsync(usernameOrEmail);
        }

        if (user == null)
        {
            return new AuthenticationResult { Success = false, Message = "Invalid username/email or password." };
        }

        if (!user.IsActive)
        {
            return new AuthenticationResult { Success = false, Message = "Account is inactive." };
        }

        if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            return new AuthenticationResult { Success = false, Message = "Invalid username/email or password." };
        }

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        await _userRepository.UpdateAsync(user);

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
        var token = await _tokenRepository.GetRefreshTokenAsync(refreshToken);

        if (token == null || !token.IsActive)
        {
            return new AuthenticationResult { Success = false, Message = "Invalid or expired refresh token." };
        }

        var user = await _userRepository.GetByIdAsync(token.UserId);

        if (user == null || !user.IsActive)
        {
            return new AuthenticationResult { Success = false, Message = "User not found or inactive." };
        }

        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = await GenerateRefreshTokenAsync(user.Uuid);

        token.RevokedAt = DateTime.UtcNow;
        await _tokenRepository.UpdateRefreshTokenAsync(token);
        token.RevokedReason = "Replaced by new token";
        token.ReplacedByToken = newRefreshToken.Token;
        await _tokenRepository.UpdateRefreshTokenAsync(token);

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
        var token = await _tokenRepository.GetRefreshTokenAsync(refreshToken);

        if (token == null || token.IsRevoked)
        {
            return false;
        }

        token.RevokedAt = DateTime.UtcNow;
        token.RevokedReason = reason ?? "Manually revoked";
        await _tokenRepository.UpdateRefreshTokenAsync(token);

        return true;
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        if (!VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
        {
            return false;
        }

        var newPasswordHash = HashPassword(newPassword, out string salt);
        user.PasswordHash = newPasswordHash;
        user.PasswordSalt = salt;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(userId);

        return true;
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
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

        return token.Token;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var resetToken = await _tokenRepository.GetPasswordResetTokenAsync(token);

        if (resetToken == null || !resetToken.IsValid)
        {
            return false;
        }

        var user = await _userRepository.GetByIdAsync(resetToken.UserId);

        if (user == null)
        {
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

        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        // Token here is the user's UUID for email verification
        if (!Guid.TryParse(token, out Guid userId))
        {
            return false;
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.EmailVerified = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

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

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        // Prevent deletion of superuser
        if (user.IsSuperUser)
        {
            return false;
        }

        await _userRepository.DeleteAsync(userId);
        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(userId);
        // Note: Password reset tokens will be handled by MongoDB TTL indexes or cleanup jobs

        return true;
    }

    public async Task<bool> DeactivateUserAsync(Guid userId)
    {

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        // Prevent deactivation of superuser
        if (user.IsSuperUser)
        {
            return false;
        }

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Revoke all refresh tokens
        await _tokenRepository.DeleteRefreshTokensByUserIdAsync(userId);

        return true;
    }

    public string HashPassword(string password, out string salt)
    {
        using var hmac = new HMACSHA512();
        salt = Convert.ToBase64String(hmac.Key);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var hmac = new HMACSHA512(saltBytes);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var computedHashString = Convert.ToBase64String(computedHash);
        return computedHashString == hash;
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