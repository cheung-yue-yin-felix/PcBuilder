using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkDeleteGpus;

public class BulkDeleteGpusHandler(
    IApplicationDbContext context,
    ICacheService cache,
    ILogger<BulkDeleteGpusHandler> logger) : IRequestHandler<BulkDeleteGpusCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteGpusCommand request, CancellationToken cancellationToken)
    {
        var ids = request.GpuIds.Distinct().ToList();

        var entities = await context.Gpus
            .Where(x => ids.Contains(x.Id) && x.IsActive)
            .ToListAsync(cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Gpu, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Gpu);
        return true;
    }
}
