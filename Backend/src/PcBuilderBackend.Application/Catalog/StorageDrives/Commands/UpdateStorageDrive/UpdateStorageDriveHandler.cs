using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.UpdateStorageDrive;

public class UpdateStorageDriveHandler(
    IStorageDriveRepository storageDrives,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateStorageDriveHandler> logger)
    : IRequestHandler<UpdateStorageDriveCommand, StorageDriveDto?>
{
    public async Task<StorageDriveDto?> Handle(
        UpdateStorageDriveCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await storageDrives.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.StorageDrive, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.Media,
            request.Interface,
            request.FormFactor,
            request.CapacityGb,
            request.PcieGeneration,
            request.Rpm);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.StorageDrive, entity.Id);

        return mapper.Map<StorageDriveDto>(entity);
    }
}
