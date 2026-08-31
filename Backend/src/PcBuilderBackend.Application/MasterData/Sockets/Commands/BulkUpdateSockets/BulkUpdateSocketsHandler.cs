using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkUpdateSockets;

public class BulkUpdateSocketsHandler(
    IRepository<Socket> sockets,
    ILogger<BulkUpdateSocketsHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cache
)
    : IRequestHandler<BulkUpdateSocketsCommand, List<SocketDto>>
{
    public async Task<List<SocketDto>> Handle(BulkUpdateSocketsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<SocketDto>();

        foreach (var socket in request.Sockets)
        {
            var entity = await sockets.GetByIdAsync(socket.Id, cancellationToken);
            if (entity is null) return result;
            entity.Rename(socket.Name);
            entity.UpdateManufacturer(socket.ManufacturerId);
            result.Add(mapper.Map<SocketDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Socket);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return result;
    }
}
