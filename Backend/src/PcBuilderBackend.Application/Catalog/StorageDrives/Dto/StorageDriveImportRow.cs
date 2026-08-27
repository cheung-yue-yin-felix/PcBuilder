using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

public class StorageDriveImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public StorageMedia Media { get; init; }
    public StorageInterface Interface { get; init; }
    public StorageFormFactor FormFactor { get; init; }
    public int CapacityGb { get; init; }
    public PcieGeneration? PcieGeneration { get; init; }
    public int? Rpm { get; init; }
}
