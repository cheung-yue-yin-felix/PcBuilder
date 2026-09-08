namespace PcBuilderBackend.Application.Common.Options;

public sealed class AppOptions
{
    public const string SectionName = "App";

    public string PublicBaseUrl { get; set; } = "http://localhost:5173";
}
