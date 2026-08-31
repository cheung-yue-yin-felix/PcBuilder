using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;

public class BulkDeleteCpuSeriesHandler(
    IRepository<Domain.Entities.CpuSeries> cpuSeries, 
    ILogger<BulkDeleteCpuSeriesHandler> logger,
    IUnitOfWork unitOfWork, 
    ICacheService cache)
    : IRequestHandler<BulkDeleteCpuSeriesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteCpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var ids = command.Ids.Distinct().ToList();
        var entities = await cpuSeries.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.CpuSeries, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, ids.Count, EntityLog.CpuSeries);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return true;
    }
}
