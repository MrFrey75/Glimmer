namespace Glimmer.Core.Models;

public class AuthenticationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public User? User { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
