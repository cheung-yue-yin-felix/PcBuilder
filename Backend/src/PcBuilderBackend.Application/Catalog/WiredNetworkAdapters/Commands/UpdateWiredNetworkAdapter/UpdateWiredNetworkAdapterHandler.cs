using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.UpdateWiredNetworkAdapter;

public class UpdateWiredNetworkAdapterHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<UpdateWiredNetworkAdapterHandler> logger)
    : IRequestHandler<UpdateWiredNetworkAdapterCommand, WiredNetworkAdapterDto?>
{
    public async Task<WiredNetworkAdapterDto?> Handle(
        UpdateWiredNetworkAdapterCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.WiredNetworkAdapters
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

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

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.WiredNetworkAdapter, entity.Id);

        return mapper.Map<WiredNetworkAdapterDto>(entity);
    }
}
