using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Build;

public sealed record CompatibilityPartRef(CompatibilitySlot Slot, Guid? PartId);

public sealed record CompatibilityCheck(
    PartsCompatibilityResult Result,
    IReadOnlyList<CompatibilityPartRef> Parts);
