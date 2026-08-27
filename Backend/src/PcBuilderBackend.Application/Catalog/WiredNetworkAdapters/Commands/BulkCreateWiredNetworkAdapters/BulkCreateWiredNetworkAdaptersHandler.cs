using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkCreateWiredNetworkAdapters;

public class BulkCreateWiredNetworkAdaptersHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkCreateWiredNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkCreateWiredNetworkAdaptersCommand, List<WiredNetworkAdapterDto>>
{
    public async Task<List<WiredNetworkAdapterDto>> Handle(
        BulkCreateWiredNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<WiredNetworkAdapterDto>();

        foreach (var entity in request.Adapters.Select(item => new WiredNetworkAdapter(
                     item.Name,
                     item.ManufacturerId,
                     item.HostInterface,
                     item.MaxSpeedMbps,
                     item.UsbVersion,
                     item.UsbType,
                     item.PcieSlotType)))
        {
            context.WiredNetworkAdapters.Add(entity);
            result.Add(mapper.Map<WiredNetworkAdapterDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.WiredNetworkAdapter);
        return result;
    }
}
