using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives;

public interface IStorageDriveFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    StorageMedia Media { get; }
    StorageInterface Interface { get; }
    StorageFormFactor FormFactor { get; }
    int CapacityGb { get; }
    PcieGeneration? PcieGeneration { get; }
    int? Rpm { get; }
}
