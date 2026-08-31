using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public class GetChipsetsHandler(IReadStore<Chipset, ChipsetDto> store, ICacheService cache)
    : IRequestHandler<GetChipsetsQuery, List<ChipsetDto>>
{
    public Task<List<ChipsetDto>> Handle(GetChipsetsQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.Chipsets.All(),
            store.ListAsync,
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
