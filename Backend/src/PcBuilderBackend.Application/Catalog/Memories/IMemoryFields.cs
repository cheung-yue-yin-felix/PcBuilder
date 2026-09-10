using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Memories;

public interface IMemoryFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    string Color { get; }
    DdrGeneration DdrGeneration { get; }
    RamFormFactor RamFormFactor { get; }
    RamRank RamRank { get; }
    int MemorySizePerStickGb { get; }
    int TotalMemorySizeGb { get; }
    int ModulesCount { get; }
    int MaxMemorySpeedMts { get; }
    decimal HeightMm { get; }
}
