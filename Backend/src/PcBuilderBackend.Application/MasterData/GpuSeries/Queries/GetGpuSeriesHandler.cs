using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public class GetGpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetGpuSeriesQuery, List<GpuSeriesDto>>
{
    public Task<List<GpuSeriesDto>> Handle(GetGpuSeriesQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.GpuSeries.All(),
            ct => context.GpuSeries
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ProjectTo<GpuSeriesDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct),
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
