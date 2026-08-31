using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkDeleteManufacturers;

public class BulkDeleteManufacturersHandler(
    IRepository<Manufacturer> manufacturers, 
    ILogger<BulkDeleteManufacturersHandler> logger,
    IUnitOfWork unitOfWork, 
    ICacheService cache)
    : IRequestHandler<BulkDeleteManufacturersCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteManufacturersCommand command, CancellationToken cancellationToken)
    {
        var ids = command.Ids.Distinct().ToList();
        var entities = await manufacturers.GetByIdsAsync(ids, cancellationToken);

        if (entities.Count != ids.Count) return false;

        foreach (var manufacturer in entities)
        {
            manufacturer.Deactivate();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkDeleted(logger, entities.Count, EntityLog.Manufacturer);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);
        return true;
    }
}
