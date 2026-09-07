using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public class GetGpuSeriesByIdHandler(IReadStore<GpuSeriesDto> store, ICacheService cache)
    : IRequestHandler<GetGpuSeriesByIdQuery, GpuSeriesDto?>
{
    public async Task<GpuSeriesDto?> Handle(GetGpuSeriesByIdQuery query, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.GpuSeries.ById(query.Id);
        var cached = await cache.GetAsync<GpuSeriesDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await store.GetByIdAsync(query.Id, cancellationToken);

        if (dto is null)
            return null;

        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
