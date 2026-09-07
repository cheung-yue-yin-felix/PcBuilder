using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkUpdateStorageDrives;

public class BulkUpdateStorageDrivesHandler(
    IStorageDriveRepository storageDrives,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateStorageDrivesHandler> logger)
    : IRequestHandler<BulkUpdateStorageDrivesCommand, List<StorageDriveDto>?>
{
    public async Task<List<StorageDriveDto>?> Handle(
        BulkUpdateStorageDrivesCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<StorageDriveDto>();

        foreach (var item in request.Drives)
        {
            var entity = await storageDrives.GetByIdAsync(item.Id, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.StorageDrive, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(new StorageDriveSpecs
            {
                Media = item.Media,
                Interface = item.Interface,
                FormFactor = item.FormFactor,
                CapacityGb = item.CapacityGb,
                PcieGeneration = item.PcieGeneration,
                Rpm = item.Rpm
            });

            result.Add(mapper.Map<StorageDriveDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.StorageDrive);
        return result;
    }
}
