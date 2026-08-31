using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.DeleteSocket;

public class DeleteSocketHandler(
    IRepository<Socket> sockets,
    ILogger<DeleteSocketHandler> logger,
    IUnitOfWork unitOfWork,
    ICacheService cache
)
    : IRequestHandler<DeleteSocketCommand, bool>
{
    public async Task<bool> Handle(DeleteSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = await sockets.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        entity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Deleted(logger, EntityLog.Socket, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return true;
    }
}
