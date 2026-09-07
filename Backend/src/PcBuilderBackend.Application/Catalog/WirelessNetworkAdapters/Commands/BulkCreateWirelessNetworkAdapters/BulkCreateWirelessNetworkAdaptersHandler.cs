using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

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
                     new WirelessNetworkAdapterSpecs
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
                     })))
        {
            adapters.Add(entity);
            result.Add(mapper.Map<WirelessNetworkAdapterDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.WirelessNetworkAdapter);
        return result;
    }
}
