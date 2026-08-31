using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public class GetCpuSeriesByIdHandler(IReadStore<Domain.Entities.CpuSeries, CpuSeriesDto> store, ICacheService cache)
    : IRequestHandler<GetCpuSeriesByIdQuery, CpuSeriesDto?>
{
    public async Task<CpuSeriesDto?> Handle(GetCpuSeriesByIdQuery query, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.CpuSeries.ById(query.Id);
        var cached = await cache.GetAsync<CpuSeriesDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var result = await store.GetByIdAsync(query.Id, cancellationToken);

        if (result is null)
            return null;

        await cache.SetAsync(key, result, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return result;
    }
}
