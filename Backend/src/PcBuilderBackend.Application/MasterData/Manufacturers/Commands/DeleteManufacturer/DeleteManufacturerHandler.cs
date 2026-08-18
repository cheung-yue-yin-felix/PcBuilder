using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.DeleteManufacturer;

public class DeleteManufacturerHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<DeleteManufacturerCommand, bool>
{
    public async Task<bool> Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers.FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);
        if (entity == null) return false;

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);
        return true;
    }
}
