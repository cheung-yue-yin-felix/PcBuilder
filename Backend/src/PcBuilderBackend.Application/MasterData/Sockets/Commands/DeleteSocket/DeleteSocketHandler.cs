using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.DeleteSocket;

public class DeleteSocketHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<DeleteSocketCommand, bool>
{
    public async Task<bool> Handle(DeleteSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Sockets.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);
        if (entity is null) return false;

        entity.Deactivate();
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return true;
    }
}
