using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpuByIdHandler(IReadStore<Domain.Entities.Gpu, GpuDto> store, ICacheService cache)
    : IRequestHandler<GetGpuByIdQuery, GpuDto?>
{
    public async Task<GpuDto?> Handle(GetGpuByIdQuery query, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Gpus.ById(query.Id);
        var cached = await cache.GetAsync<GpuDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await store.GetByIdAsync(query.Id, cancellationToken);

        if (dto is null)
            return null;

        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
