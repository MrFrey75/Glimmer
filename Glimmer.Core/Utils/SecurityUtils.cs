namespace Glimmer.Core.Utils;

using System.Security.Cryptography;
using System.Text;

public static class SecurityUtils
{
    /// <summary>
    /// Hashes a password using HMACSHA512 with a cryptographically secure salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A tuple containing the password hash and the salt, both as Base64-encoded strings.</returns>
    public static (string hash, string salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    /// <summary>
    /// Verifies a password against a stored hash and salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hash">The stored password hash (Base64-encoded).</param>
    /// <param name="salt">The stored salt (Base64-encoded).</param>
    /// <returns>True if the password is correct, otherwise false.</returns>
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var hashBytes = Convert.FromBase64String(hash);
        using var hmac = new HMACSHA512(saltBytes);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hashBytes);
    }
}
