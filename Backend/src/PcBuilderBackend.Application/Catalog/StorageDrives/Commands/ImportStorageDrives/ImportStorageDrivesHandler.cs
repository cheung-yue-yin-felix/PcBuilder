using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.ImportStorageDrives;

public class ImportStorageDrivesHandler(
    IApplicationDbContext context,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportStorageDrivesHandler> logger)
    : IRequestHandler<ImportStorageDrivesCommand, List<StorageDriveDto>>
{
    public async Task<List<StorageDriveDto>> Handle(
        ImportStorageDrivesCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseStorageDriveImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<StorageDrive>();

        foreach (var row in rows)
        {
            var entity = new StorageDrive(
                row.Name,
                row.ManufacturerId,
                row.Media,
                row.Interface,
                row.FormFactor,
                row.CapacityGb,
                row.PcieGeneration,
                row.Rpm);

            context.StorageDrives.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.StorageDrive);

        return [.. result.Select(mapper.Map<StorageDriveDto>)];
    }
}
