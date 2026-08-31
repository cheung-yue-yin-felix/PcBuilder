using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public class GetCpuSeriesHandler(IReadStore<Domain.Entities.CpuSeries, CpuSeriesDto> store, ICacheService cache)
    : IRequestHandler<GetCpuSeriesQuery, List<CpuSeriesDto>>
{
    public Task<List<CpuSeriesDto>> Handle(GetCpuSeriesQuery query, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.CpuSeries.All(),
            store.ListAsync,
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
