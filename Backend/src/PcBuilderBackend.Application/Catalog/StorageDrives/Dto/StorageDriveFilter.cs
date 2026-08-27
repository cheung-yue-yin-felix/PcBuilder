using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

public record StorageDriveFilter
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public StorageMedia? Media { get; init; }
    public StorageInterface? Interface { get; init; }
    public StorageFormFactor? FormFactor { get; init; }
    public RangeFilter? CapacityGb { get; init; }
    public PcieGeneration? PcieGeneration { get; init; }
    public RangeFilter? Rpm { get; init; }
    public Guid? MotherboardId { get; init; }
    public Guid? ChassisId { get; init; }
}
