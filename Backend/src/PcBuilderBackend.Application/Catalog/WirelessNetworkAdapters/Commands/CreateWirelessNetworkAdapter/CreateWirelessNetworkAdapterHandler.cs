using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.CreateWirelessNetworkAdapter;

public class CreateWirelessNetworkAdapterHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<CreateWirelessNetworkAdapterHandler> logger)
    : IRequestHandler<CreateWirelessNetworkAdapterCommand, WirelessNetworkAdapterDto>
{
    public async Task<WirelessNetworkAdapterDto> Handle(
        CreateWirelessNetworkAdapterCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new WirelessNetworkAdapter(
            request.Name,
            request.ManufacturerId,
            request.WifiStandard,
            request.HostInterface,
            request.MaxSpeedMbps,
            request.MaxSpeedMbps5G,
            request.MaxSpeedMbps6G,
            request.BluetoothVersion,
            request.PcieSlotType,
            request.Key,
            request.M2FormFactor,
            request.UsbVersion,
            request.UsbType);

        context.WirelessNetworkAdapters.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.WirelessNetworkAdapter, entity.Id);

        return mapper.Map<WirelessNetworkAdapterDto>(entity);
    }
}
