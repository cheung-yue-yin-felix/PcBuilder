using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkDeleteGpus;

public class BulkDeleteGpusHandler(
    IRepository<Gpu> gpus,
    IUnitOfWork unitOfWork,
    ICacheService cache,
    ILogger<BulkDeleteGpusHandler> logger) : IRequestHandler<BulkDeleteGpusCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteGpusCommand command, CancellationToken cancellationToken)
    {
        var ids = command.Ids.Distinct().ToList();

        var entities = await gpus.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Gpu, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Gpu);
        return true;
    }
}
