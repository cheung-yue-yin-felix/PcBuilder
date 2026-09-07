using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkUpdateWirelessNetworkAdapters;

public class BulkUpdateWirelessNetworkAdaptersHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
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
            var entity = await adapters.GetByIdAsync(item.Id, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.WirelessNetworkAdapter, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(new WirelessNetworkAdapterSpecs
            {
                WifiStandard = item.WifiStandard,
                HostInterface = item.HostInterface,
                MaxSpeedMbps = item.MaxSpeedMbps,
                MaxSpeedMbps5G = item.MaxSpeedMbps5G,
                MaxSpeedMbps6G = item.MaxSpeedMbps6G,
                BluetoothVersion = item.BluetoothVersion,
                PcieSlotType = item.PcieSlotType,
                M2Key = item.Key,
                M2FormFactor = item.M2FormFactor,
                UsbVersion = item.UsbVersion,
                UsbType = item.UsbType
            });

            result.Add(mapper.Map<WirelessNetworkAdapterDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.WirelessNetworkAdapter);
        return result;
    }
}
