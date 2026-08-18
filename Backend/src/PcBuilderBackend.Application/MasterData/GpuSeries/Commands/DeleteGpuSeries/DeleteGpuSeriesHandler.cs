using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.DeleteGpuSeries;

public class DeleteGpuSeriesHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<DeleteGpuSeriesCommand, bool>
{
    public async Task<bool> Handle(DeleteGpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GpuSeries.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsActive, cancellationToken);
        if (entity == null) return false;

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return true;
    }
}
