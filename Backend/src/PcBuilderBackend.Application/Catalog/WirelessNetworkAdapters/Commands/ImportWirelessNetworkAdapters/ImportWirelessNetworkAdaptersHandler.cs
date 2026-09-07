using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.ImportWirelessNetworkAdapters;

public class ImportWirelessNetworkAdaptersHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
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
                new WirelessNetworkAdapterSpecs
                {
                    WifiStandard = row.WifiStandard,
                    HostInterface = row.HostInterface,
                    MaxSpeedMbps = row.MaxSpeedMbps,
                    MaxSpeedMbps5G = row.MaxSpeedMbps5G,
                    MaxSpeedMbps6G = row.MaxSpeedMbps6G,
                    BluetoothVersion = row.BluetoothVersion,
                    PcieSlotType = row.PcieSlotType,
                    M2Key = row.Key,
                    M2FormFactor = row.M2FormFactor,
                    UsbVersion = row.UsbVersion,
                    UsbType = row.UsbType
                });

            adapters.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.WirelessNetworkAdapter);

        return [.. result.Select(mapper.Map<WirelessNetworkAdapterDto>)];
    }
}
