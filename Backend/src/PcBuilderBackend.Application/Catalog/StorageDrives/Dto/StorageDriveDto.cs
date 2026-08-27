using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

public record StorageDriveDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public string ManufacturerName { get; init; } = string.Empty;
    public StorageMedia Media { get; init; }
    public StorageInterface Interface { get; init; }
    public StorageFormFactor FormFactor { get; init; }
    public int CapacityGb { get; init; }
    public PcieGeneration? PcieGeneration { get; init; }
    public int? Rpm { get; init; }
    public bool IsM2 { get; init; }
    public M2Key? ModuleKey { get; init; }
    public M2FormFactor? M2FormFactor { get; init; }
}
