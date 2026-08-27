using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkDeleteWirelessNetworkAdapters;

public class BulkDeleteWirelessNetworkAdaptersHandler(
    IApplicationDbContext context,
    ILogger<BulkDeleteWirelessNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkDeleteWirelessNetworkAdaptersCommand, bool>
{
    public async Task<bool> Handle(
        BulkDeleteWirelessNetworkAdaptersCommand request,
        CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();

        var entities = await context.WirelessNetworkAdapters
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.WirelessNetworkAdapter, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.WirelessNetworkAdapter);
        return true;
    }
}
