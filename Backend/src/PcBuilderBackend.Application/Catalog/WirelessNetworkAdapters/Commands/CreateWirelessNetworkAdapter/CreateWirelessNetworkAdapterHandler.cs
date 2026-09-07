using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.CreateWirelessNetworkAdapter;

public class CreateWirelessNetworkAdapterHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
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
            new WirelessNetworkAdapterSpecs
            {
                WifiStandard = request.WifiStandard,
                HostInterface = request.HostInterface,
                MaxSpeedMbps = request.MaxSpeedMbps,
                MaxSpeedMbps5G = request.MaxSpeedMbps5G,
                MaxSpeedMbps6G = request.MaxSpeedMbps6G,
                BluetoothVersion = request.BluetoothVersion,
                PcieSlotType = request.PcieSlotType,
                M2Key = request.Key,
                M2FormFactor = request.M2FormFactor,
                UsbVersion = request.UsbVersion,
                UsbType = request.UsbType
            });

        adapters.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.WirelessNetworkAdapter, entity.Id);

        return mapper.Map<WirelessNetworkAdapterDto>(entity);
    }
}
