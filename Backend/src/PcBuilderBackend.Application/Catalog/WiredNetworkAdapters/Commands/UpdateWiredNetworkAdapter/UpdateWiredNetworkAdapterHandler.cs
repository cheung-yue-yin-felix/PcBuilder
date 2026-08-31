using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.UpdateWiredNetworkAdapter;

public class UpdateWiredNetworkAdapterHandler(
    IWiredNetworkAdapterRepository adapters,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateWiredNetworkAdapterHandler> logger)
    : IRequestHandler<UpdateWiredNetworkAdapterCommand, WiredNetworkAdapterDto?>
{
    public async Task<WiredNetworkAdapterDto?> Handle(
        UpdateWiredNetworkAdapterCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await adapters.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.WiredNetworkAdapter, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.HostInterface,
            request.MaxSpeedMbps,
            request.UsbVersion,
            request.UsbType,
            request.PcieSlotType);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.WiredNetworkAdapter, entity.Id);

        return mapper.Map<WiredNetworkAdapterDto>(entity);
    }
}
