using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public class GetCpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetCpuSeriesQuery, List<CpuSeriesDto>>
{
    public Task<List<CpuSeriesDto>> Handle(GetCpuSeriesQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.CpuSeries.All(),
            ct => context.CpuSeries
                .AsNoTracking()
                .Where(cs => cs.IsActive)
                .ProjectTo<CpuSeriesDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct),
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
