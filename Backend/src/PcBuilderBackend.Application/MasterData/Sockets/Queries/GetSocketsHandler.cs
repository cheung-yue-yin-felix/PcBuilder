using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public class GetSocketsHandler(IReadStore<SocketDto> store, ICacheService cache)
    : IRequestHandler<GetSocketsQuery, List<SocketDto>>
{
    public Task<List<SocketDto>> Handle(GetSocketsQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.Sockets.All(),
            store.ListAsync,
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
