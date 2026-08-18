using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;

public class BulkDeleteCpuSeriesHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<BulkDeleteCpuSeriesCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteCpuSeriesCommand request, CancellationToken cancellationToken)
    {
        foreach (var cpuSeriesId in request.CpuSeriesIds)
        {
            var entity = await context.CpuSeries.FirstOrDefaultAsync(cs => cs.Id == cpuSeriesId && cs.IsActive, cancellationToken);

            if (entity is null) return false;

            entity.Deactivate();
        }
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return true;
    }
}
