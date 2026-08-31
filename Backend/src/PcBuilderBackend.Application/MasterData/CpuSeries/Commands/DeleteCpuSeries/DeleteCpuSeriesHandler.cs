using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.DeleteCpuSeries;

public class DeleteCpuSeriesHandler(IRepository<Domain.Entities.CpuSeries> cpuSeries, IUnitOfWork unitOfWork, ICacheService cache)
    : IRequestHandler<DeleteCpuSeriesCommand, bool>
{
    public async Task<bool> Handle(DeleteCpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var entity = await cpuSeries.GetByIdAsync(command.CpuSeriesId, cancellationToken);
        if (entity is null) return false;
        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return true;
    }
}
