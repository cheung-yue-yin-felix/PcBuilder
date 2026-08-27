using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.ImportWiredNetworkAdapters;

public class ImportWiredNetworkAdaptersHandler(
    IApplicationDbContext context,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportWiredNetworkAdaptersHandler> logger)
    : IRequestHandler<ImportWiredNetworkAdaptersCommand, List<WiredNetworkAdapterDto>>
{
    public async Task<List<WiredNetworkAdapterDto>> Handle(
        ImportWiredNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseWiredNetworkAdapterImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<WiredNetworkAdapter>();

        foreach (var row in rows)
        {
            var entity = new WiredNetworkAdapter(
                row.Name,
                row.ManufacturerId,
                row.HostInterface,
                row.MaxSpeedMbps,
                row.UsbVersion,
                row.UsbType,
                row.PcieSlotType);

            context.WiredNetworkAdapters.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.WiredNetworkAdapter);

        return [.. result.Select(mapper.Map<WiredNetworkAdapterDto>)];
    }
}
