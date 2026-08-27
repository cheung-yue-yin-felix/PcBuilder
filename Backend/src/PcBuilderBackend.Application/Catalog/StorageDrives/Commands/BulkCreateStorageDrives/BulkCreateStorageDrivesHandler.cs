using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkCreateStorageDrives;

public class BulkCreateStorageDrivesHandler(
    IApplicationDbContext context,
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
                     item.Media,
                     item.Interface,
                     item.FormFactor,
                     item.CapacityGb,
                     item.PcieGeneration,
                     item.Rpm)))
        {
            context.StorageDrives.Add(entity);
            result.Add(mapper.Map<StorageDriveDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.StorageDrive);
        return result;
    }
}
