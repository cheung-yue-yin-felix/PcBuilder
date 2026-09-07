using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.ValueObjects;

public sealed class StorageDriveSpecs
{
    public StorageMedia Media { get; init; }
    public StorageInterface Interface { get; init; }
    public StorageFormFactor FormFactor { get; init; }
    public int CapacityGb { get; init; }
    public PcieGeneration? PcieGeneration { get; init; }
    public int? Rpm { get; init; }
}
