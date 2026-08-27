namespace PcBuilderBackend.Application.Common.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer))
            throw new InvalidOperationException("Jwt:Issuer is required.");
        if (string.IsNullOrWhiteSpace(Audience))
            throw new InvalidOperationException("Jwt:Audience is required.");
        if (string.IsNullOrWhiteSpace(Key) || Key.Length < 32)
            throw new InvalidOperationException("Jwt:Key must be at least 32 characters.");
        if (AccessTokenMinutes <= 0)
            throw new InvalidOperationException("Jwt:AccessTokenMinutes must be greater than 0.");
        if (RefreshTokenDays <= 0)
            throw new InvalidOperationException("Jwt:RefreshTokenDays must be greater than 0.");
    }
}
