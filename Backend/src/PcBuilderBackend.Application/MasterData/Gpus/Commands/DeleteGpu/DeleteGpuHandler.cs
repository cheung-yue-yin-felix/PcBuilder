using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.DeleteGpu;

public class DeleteGpuHandler(
    IRepository<Domain.Entities.Gpu> gpus,
    IUnitOfWork unitOfWork,
    ICacheService cache, 
    ILogger<DeleteGpuHandler> logger)
    : IRequestHandler<DeleteGpuCommand, bool>
{
    public async Task<bool> Handle(DeleteGpuCommand request, CancellationToken cancellationToken)
    {
        var gpu = await gpus.GetByIdAsync(request.Id, cancellationToken);
        if (gpu is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Gpu, request.Id);
            return false;
        }

        gpu.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        EntityLog.Deleted(logger, EntityLog.Gpu, request.Id);
        return true;
    }
}
