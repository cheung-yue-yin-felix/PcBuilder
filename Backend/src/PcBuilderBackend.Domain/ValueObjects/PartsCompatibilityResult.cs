using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.ValueObjects;

/// <summary>
/// Compatibility outcome. Values (rated/executing) exist only for
/// <see cref="PartsCompatibility.CompatibleReduced"/>; all other statuses are valueless.
/// </summary>
public abstract class PartsCompatibilityResult
{
    protected PartsCompatibilityResult(PartsCompatibility status, CompatibilityReason reason)
    {
        Status = status;
        Reason = reason;
    }

    public PartsCompatibility Status { get; }
    public CompatibilityReason Reason { get; }

    public static PartsCompatibilityResult Compatible()
    {
        return Valueless.Create(PartsCompatibility.Compatible, CompatibilityReason.None);
    }

    public static PartsCompatibilityResult Incompatible(CompatibilityReason reason)
    {
        return Valueless.Create(PartsCompatibility.Incompatible, reason);
    }

    public static PartsCompatibilityResult CompatibleActionRequired(CompatibilityReason reason)
    {
        return Valueless.Create(PartsCompatibility.CompatibleActionRequired, reason);
    }

    /// <summary>
    /// Compatible, but constrained: <paramref name="rated"/> is the advertised capability,
    /// <paramref name="executing"/> is what actually runs under the other part.
    /// </summary>
    public static PartsCompatibilityResult<T> CompatibleReduced<T>(
        CompatibilityReason reason,
        T rated,
        T executing)
    {
        return new PartsCompatibilityResult<T>(reason, rated, executing);
    }

    private sealed class Valueless : PartsCompatibilityResult
    {
        private Valueless(PartsCompatibility status, CompatibilityReason reason)
            : base(status, reason)
        {
        }

        public static Valueless Create(PartsCompatibility status, CompatibilityReason reason)
        {
            return new Valueless(status, reason);
        }
    }
}

/// <summary>
/// Compatible-with-reduced-performance only. Carries rated vs executing values.
/// </summary>
public sealed class PartsCompatibilityResult<T> : PartsCompatibilityResult
{
    internal PartsCompatibilityResult(CompatibilityReason reason, T rated, T executing)
        : base(PartsCompatibility.CompatibleReduced, reason)
    {
        Rated = rated;
        Executing = executing;
    }

    /// <summary>Advertised / capable value of the constrained part.</summary>
    public T Rated { get; }

    /// <summary>Actual operating value under the other part's limit.</summary>
    public T Executing { get; }
}
