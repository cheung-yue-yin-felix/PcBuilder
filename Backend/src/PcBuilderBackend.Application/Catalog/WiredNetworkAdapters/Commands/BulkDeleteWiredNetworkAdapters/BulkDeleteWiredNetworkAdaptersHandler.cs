using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkDeleteWiredNetworkAdapters;

public class BulkDeleteWiredNetworkAdaptersHandler(
    IApplicationDbContext context,
    ILogger<BulkDeleteWiredNetworkAdaptersHandler> logger)
    : IRequestHandler<BulkDeleteWiredNetworkAdaptersCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteWiredNetworkAdaptersCommand request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();

        var entities = await context.WiredNetworkAdapters
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.WiredNetworkAdapter, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
            entity.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.WiredNetworkAdapter);
        return true;
    }
}
