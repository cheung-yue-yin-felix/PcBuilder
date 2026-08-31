using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.DeleteGpuSeries;

public class DeleteGpuSeriesHandler(
    IRepository<Domain.Entities.GpuSeries> gpuSeries, 
    ILogger<DeleteGpuSeriesHandler> logger,
    IUnitOfWork unitOfWork, 
    ICacheService cache)
    : IRequestHandler<DeleteGpuSeriesCommand, bool>
{
    public async Task<bool> Handle(DeleteGpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var entity = await gpuSeries.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null) return false;

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Deleted(logger, EntityLog.GpuSeries, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return true;
    }
}
