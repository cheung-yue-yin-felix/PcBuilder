using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public class GetSocketByIdHandler(IReadStore<Socket, SocketDto> store, ICacheService cache)
    : IRequestHandler<GetSocketByIdQuery, SocketDto?>
{
    public async Task<SocketDto?> Handle(GetSocketByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Sockets.ById(request.Id);
        var cached = await cache.GetAsync<SocketDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await store.GetByIdAsync(request.Id, cancellationToken);

        if (dto is null)
            return null;

        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
