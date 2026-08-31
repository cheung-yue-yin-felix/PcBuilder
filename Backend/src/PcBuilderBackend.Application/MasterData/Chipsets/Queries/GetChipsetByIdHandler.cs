using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public class GetChipsetByIdHandler(IReadStore<Chipset, ChipsetDto> store, ICacheService cache)
    : IRequestHandler<GetChipsetByIdQuery, ChipsetDto?>
{
    public async Task<ChipsetDto?> Handle(GetChipsetByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Chipsets.ById(request.Id);
        var cached = await cache.GetAsync<ChipsetDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var chipset = await store.GetByIdAsync(request.Id, cancellationToken);

        if (chipset is null)
            return null;

        await cache.SetAsync(key, chipset, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return chipset;
    }
}
