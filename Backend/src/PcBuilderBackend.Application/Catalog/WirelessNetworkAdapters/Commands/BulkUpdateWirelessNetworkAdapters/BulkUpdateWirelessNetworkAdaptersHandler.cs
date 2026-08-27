using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkUpdateWirelessNetworkAdapters;

public class BulkUpdateWirelessNetworkAdaptersHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateWirelessNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkUpdateWirelessNetworkAdaptersCommand, List<WirelessNetworkAdapterDto>?>
{
    public async Task<List<WirelessNetworkAdapterDto>?> Handle(
        BulkUpdateWirelessNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<WirelessNetworkAdapterDto>();

        foreach (var item in request.Adapters)
        {
            var entity = await context.WirelessNetworkAdapters
                .FirstOrDefaultAsync(x => x.Id == item.Id && x.IsActive, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.WirelessNetworkAdapter, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(
                item.WifiStandard,
                item.HostInterface,
                item.MaxSpeedMbps,
                item.MaxSpeedMbps5G,
                item.MaxSpeedMbps6G,
                item.BluetoothVersion,
                item.PcieSlotType,
                item.Key,
                item.M2FormFactor,
                item.UsbVersion,
                item.UsbType);

            result.Add(mapper.Map<WirelessNetworkAdapterDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.WirelessNetworkAdapter);
        return result;
    }
}
