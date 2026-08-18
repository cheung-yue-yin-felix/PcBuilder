using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.DeleteChipset;

public class DeleteChipsetHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<DeleteChipsetCommand, bool>
{
    public async Task<bool> Handle(DeleteChipsetCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Chipsets.FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive, cancellationToken);
        if (entity == null) return false;
        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return true;
    }
}
