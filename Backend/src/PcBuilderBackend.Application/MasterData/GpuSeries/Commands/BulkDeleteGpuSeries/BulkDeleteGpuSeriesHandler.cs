using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkDeleteGpuSeries;

public class BulkDeleteGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries,
    ILogger<BulkDeleteGpuSeriesHandler> logger,
    IUnitOfWork context,
    ICacheService cache)
    : IRequestHandler<BulkDeleteGpuSeriesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteGpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var ids = command.Ids.Distinct().ToList();
        var entities = await gpuSeries.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count) return false;

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, ids.Count, EntityLog.GpuSeries);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return true;
    }
}
