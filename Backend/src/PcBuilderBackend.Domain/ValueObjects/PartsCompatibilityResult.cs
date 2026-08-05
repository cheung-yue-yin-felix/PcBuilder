using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.ValueObjects;

public sealed class PartsCompatibilityResult
{
    public PartsCompatibility Status { get; }
    public CompatibilityReason Reason { get; }
    public string Message { get; }

    public static PartsCompatibilityResult Compatible(string? message = null)
    {
        return new PartsCompatibilityResult(PartsCompatibility.Compatible, CompatibilityReason.None, message ?? string.Empty);
    }

    public static PartsCompatibilityResult CompatibleReduced(CompatibilityReason reason, string? message = null)
    {
        return new PartsCompatibilityResult(PartsCompatibility.CompatibleReduced, reason, message ?? string.Empty);
    }

    public static PartsCompatibilityResult Incompatible(CompatibilityReason reason, string? message = null)
    {
        return new PartsCompatibilityResult(PartsCompatibility.Incompatible, reason, message ?? string.Empty);
    }

    private PartsCompatibilityResult(PartsCompatibility status, CompatibilityReason reason, string message)
    {
        Status = status;
        Reason = reason;
        Message = message;
    }
}