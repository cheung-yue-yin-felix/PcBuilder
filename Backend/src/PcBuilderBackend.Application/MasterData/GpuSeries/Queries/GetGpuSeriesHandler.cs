using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public class GetGpuSeriesHandler(IReadStore<GpuSeriesDto> store, ICacheService cache)
    : IRequestHandler<GetGpuSeriesQuery, List<GpuSeriesDto>>
{
    public Task<List<GpuSeriesDto>> Handle(GetGpuSeriesQuery query, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.GpuSeries.All(),
            store.ListAsync,
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
