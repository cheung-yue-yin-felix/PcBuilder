using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkCreateSockets;

public class BulkCreateSocketsHandler(
    IRepository<Socket> sockets,
    ILogger<BulkCreateSocketsHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<BulkCreateSocketsCommand, List<SocketDto>>
{
    public async Task<List<SocketDto>> Handle(BulkCreateSocketsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<SocketDto>();

        foreach (var entity in request.Sockets.Select(x => new Socket(x.ManufacturerId, x.Name)))
        {
            sockets.Add(entity);
            result.Add(mapper.Map<SocketDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Socket);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return result;
    }
}
