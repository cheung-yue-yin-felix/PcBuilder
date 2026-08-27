using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.UpdateStorageDrive;

public record UpdateStorageDriveCommand : IRequest<StorageDriveDto?>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public StorageMedia Media { get; init; }
    public StorageInterface Interface { get; init; }
    public StorageFormFactor FormFactor { get; init; }
    public int CapacityGb { get; init; }
    public PcieGeneration? PcieGeneration { get; init; }
    public int? Rpm { get; init; }
}
