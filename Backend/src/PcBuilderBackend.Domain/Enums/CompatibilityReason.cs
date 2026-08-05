namespace PcBuilderBackend.Domain.Enums;

public enum CompatibilityReason
{
    None = 0,
    NoMatchingRamConfig,
    MemorySpeedExceedsCpuSupport,
    CapacityExceeded,
    NotEnoughPcieSlots,
    PcieGenerationReduced,
}