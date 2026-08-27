using MediatR;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.UpdateStorageDrive;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkUpdateStorageDrives;

public record BulkUpdateStorageDrivesCommand(List<UpdateStorageDriveCommand> Drives)
    : IRequest<List<StorageDriveDto>?>;
