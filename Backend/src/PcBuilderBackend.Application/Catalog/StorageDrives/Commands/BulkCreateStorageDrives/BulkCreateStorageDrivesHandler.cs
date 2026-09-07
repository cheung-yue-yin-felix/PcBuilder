using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkCreateStorageDrives;

public class BulkCreateStorageDrivesHandler(
    IStorageDriveRepository storageDrives,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkCreateStorageDrivesHandler> logger)
    : IRequestHandler<BulkCreateStorageDrivesCommand, List<StorageDriveDto>>
{
    public async Task<List<StorageDriveDto>> Handle(
        BulkCreateStorageDrivesCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<StorageDriveDto>();

        foreach (var entity in request.Drives.Select(item => new StorageDrive(
                     item.Name,
                     item.ManufacturerId,
                     new StorageDriveSpecs
                     {
                         Media = item.Media,
                         Interface = item.Interface,
                         FormFactor = item.FormFactor,
                         CapacityGb = item.CapacityGb,
                         PcieGeneration = item.PcieGeneration,
                         Rpm = item.Rpm
                     })))
        {
            storageDrives.Add(entity);
            result.Add(mapper.Map<StorageDriveDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.StorageDrive);
        return result;
    }
}
