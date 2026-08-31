using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkDeleteChipsets;

public class BulkDeleteChipsetsHandler(
    IRepository<Chipset> chipsets, 
    IUnitOfWork unitOfWork, 
    ILogger<BulkDeleteChipsetsHandler> logger, 
    ICacheService cache)
    : IRequestHandler<BulkDeleteChipsetsCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteChipsetsCommand command, CancellationToken cancellationToken)
    {
        var ids = command.Ids.Distinct().ToList();
        var entities = await chipsets.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count)
        {
            EntityLog.BulkAborted(logger, "delete", EntityLog.Chipset, ids.Count, entities.Count);
            return false;
        }

        foreach (var entity in entities)
        {
            entity.Deactivate();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, ids.Count, EntityLog.Chipset);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return true;
    }
}
