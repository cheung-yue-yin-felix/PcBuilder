using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerHandler(
    IRepository<Manufacturer> manufacturers,
    ILogger<DeleteManufacturerHandler> logger,
    IUnitOfWork unitOfWork,
    ICacheService cache
)
    : IRequestHandler<DeleteManufacturerCommand, bool>
{
    public async Task<bool> Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await manufacturers.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null || !entity.IsActive) return false;

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Deleted(logger, EntityLog.Manufacturer, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);
        return true;
    }
}
