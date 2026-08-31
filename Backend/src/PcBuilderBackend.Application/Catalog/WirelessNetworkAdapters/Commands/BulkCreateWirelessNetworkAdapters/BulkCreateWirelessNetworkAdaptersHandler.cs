using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkCreateWirelessNetworkAdapters;

public class BulkCreateWirelessNetworkAdaptersHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkCreateWirelessNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkCreateWirelessNetworkAdaptersCommand, List<WirelessNetworkAdapterDto>>
{
    public async Task<List<WirelessNetworkAdapterDto>> Handle(
        BulkCreateWirelessNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<WirelessNetworkAdapterDto>();

        foreach (var entity in request.Adapters.Select(item => new WirelessNetworkAdapter(
                     item.Name,
                     item.ManufacturerId,
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
                     item.UsbType)))
        {
            adapters.Add(entity);
            result.Add(mapper.Map<WirelessNetworkAdapterDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.WirelessNetworkAdapter);
        return result;
    }
}
