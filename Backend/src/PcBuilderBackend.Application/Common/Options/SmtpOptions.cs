namespace PcBuilderBackend.Application.Common.Options;

public sealed class SmtpOptions
{
    public const string SectionName = "Smtp";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string FromName { get; set; } = "PC Builder";
    public bool UseStartTls { get; set; } = true;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Host))
            throw new InvalidOperationException("Smtp:Host is required.");
        if (Port is <= 0 or > 65535)
            throw new InvalidOperationException("Smtp:Port must be between 1 and 65535.");
        if (string.IsNullOrWhiteSpace(From) || !From.Contains('@'))
            throw new InvalidOperationException("Smtp:From must be an email address.");
        if (string.IsNullOrWhiteSpace(User) != string.IsNullOrWhiteSpace(Password))
            throw new InvalidOperationException("Smtp:User and Smtp:Password must both be set or both be empty.");
        if (string.IsNullOrWhiteSpace(FromName))
            FromName = "PC Builder";
    }
}
