using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.UpdateWirelessNetworkAdapter;

public class UpdateWirelessNetworkAdapterHandler(
    IWirelessNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateWirelessNetworkAdapterHandler> logger)
    : IRequestHandler<UpdateWirelessNetworkAdapterCommand, WirelessNetworkAdapterDto?>
{
    public async Task<WirelessNetworkAdapterDto?> Handle(
        UpdateWirelessNetworkAdapterCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await adapters.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.WirelessNetworkAdapter, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
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

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.WirelessNetworkAdapter, entity.Id);

        return mapper.Map<WirelessNetworkAdapterDto>(entity);
    }
}
