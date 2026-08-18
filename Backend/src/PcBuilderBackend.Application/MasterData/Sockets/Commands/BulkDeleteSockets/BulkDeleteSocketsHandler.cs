using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkDeleteSockets;

public class BulkDeleteSocketsHandler(IApplicationDbContext context, ICacheService cache)
    : IRequestHandler<BulkDeleteSocketsCommand, bool>
{
    public async Task<bool> Handle(BulkDeleteSocketsCommand request, CancellationToken cancellationToken)
    {
        var sockets = await context.Sockets
            .Where(s => request.SocketIds.Contains(s.Id) && s.IsActive)
            .ToListAsync(cancellationToken);

        if (sockets.Count != request.SocketIds.Count) return false;

        foreach (var socket in sockets) socket.Deactivate();

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return true;
    }
}
