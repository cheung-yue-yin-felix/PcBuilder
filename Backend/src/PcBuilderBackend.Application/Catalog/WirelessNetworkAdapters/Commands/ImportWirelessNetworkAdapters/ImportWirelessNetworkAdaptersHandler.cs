using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.ImportWirelessNetworkAdapters;

public class ImportWirelessNetworkAdaptersHandler(
    IApplicationDbContext context,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportWirelessNetworkAdaptersHandler> logger)
    : IRequestHandler<ImportWirelessNetworkAdaptersCommand, List<WirelessNetworkAdapterDto>>
{
    public async Task<List<WirelessNetworkAdapterDto>> Handle(
        ImportWirelessNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseWirelessNetworkAdapterImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<WirelessNetworkAdapter>();

        foreach (var row in rows)
        {
            var entity = new WirelessNetworkAdapter(
                row.Name,
                row.ManufacturerId,
                row.WifiStandard,
                row.HostInterface,
                row.MaxSpeedMbps,
                row.MaxSpeedMbps5G,
                row.MaxSpeedMbps6G,
                row.BluetoothVersion,
                row.PcieSlotType,
                row.Key,
                row.M2FormFactor,
                row.UsbVersion,
                row.UsbType);

            context.WirelessNetworkAdapters.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.WirelessNetworkAdapter);

        return [.. result.Select(mapper.Map<WirelessNetworkAdapterDto>)];
    }
}
