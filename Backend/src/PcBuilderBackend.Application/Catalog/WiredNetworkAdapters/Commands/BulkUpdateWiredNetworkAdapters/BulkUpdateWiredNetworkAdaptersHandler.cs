using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkUpdateWiredNetworkAdapters;

public class BulkUpdateWiredNetworkAdaptersHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateWiredNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkUpdateWiredNetworkAdaptersCommand, List<WiredNetworkAdapterDto>?>
{
    public async Task<List<WiredNetworkAdapterDto>?> Handle(
        BulkUpdateWiredNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<WiredNetworkAdapterDto>();

        foreach (var item in request.Adapters)
        {
            var entity = await context.WiredNetworkAdapters
                .FirstOrDefaultAsync(x => x.Id == item.Id && x.IsActive, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.WiredNetworkAdapter, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(
                item.HostInterface,
                item.MaxSpeedMbps,
                item.UsbVersion,
                item.UsbType,
                item.PcieSlotType);

            result.Add(mapper.Map<WiredNetworkAdapterDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.WiredNetworkAdapter);
        return result;
    }
}
