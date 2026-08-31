using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

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

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.WirelessNetworkAdapter);
        return result;
    }
}
