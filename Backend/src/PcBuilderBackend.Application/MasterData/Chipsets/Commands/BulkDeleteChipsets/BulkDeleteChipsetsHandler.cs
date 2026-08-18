using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkDeleteChipsets;

public class BulkDeleteChipsetsHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<BulkDeleteChipsetsCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteChipsetsCommand request, CancellationToken cancellationToken)
    {
        foreach (var chipset in request.ChipsetIds.Select(chipsetId => context.Chipsets.FirstOrDefault(c => c.Id == chipsetId && c.IsActive)))
        {
            if (chipset == null) return false;
            chipset.Deactivate();
        }
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return true;
    }
}
