using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpusHandler(IReadStore<Domain.Entities.Gpu, GpuDto> store, ICacheService cache)
    : IRequestHandler<GetGpusQuery, List<GpuDto>>
{
    public Task<List<GpuDto>> Handle(GetGpusQuery query, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Gpus.All();

        return cache.GetOrSetAsync(
            key,
            store.ListAsync,
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
