namespace Glimmer.Core.Configuration;

public class JwtSettings
{
    public string Secret { get; set; } = "GlimmerSecretKey-ChangeInProduction-MinLength32Characters!";
    public string Issuer { get; set; } = "Glimmer.Creator";
    public string Audience { get; set; } = "Glimmer.Users";
    public int AccessTokenExpirationMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
