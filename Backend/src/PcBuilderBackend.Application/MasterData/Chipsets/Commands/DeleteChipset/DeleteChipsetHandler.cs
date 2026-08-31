using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.DeleteChipset;

public class DeleteChipsetHandler(
    IRepository<Chipset> chipsets, 
    ILogger<DeleteChipsetHandler> logger,
    IUnitOfWork unitOfWork, 
    ICacheService cache)
    : IRequestHandler<DeleteChipsetCommand, bool>
{
    public async Task<bool> Handle(DeleteChipsetCommand command, CancellationToken cancellationToken)
    {
        var entity = await chipsets.GetByIdAsync(command.Id, cancellationToken);
        if (entity == null) return false;
        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Deleted(logger, EntityLog.Chipset, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return true;
    }
}
