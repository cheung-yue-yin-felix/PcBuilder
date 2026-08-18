using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public class GetGpuSeriesByIdHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetGpuSeriesByIdQuery, GpuSeriesDto?>
{
    public async Task<GpuSeriesDto?> Handle(GetGpuSeriesByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.GpuSeries.ById(request.Id);
        var cached = await cache.GetAsync<GpuSeriesDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await context.GpuSeries
            .AsNoTracking()
            .Where(g => g.Id == request.Id && g.IsActive)
            .ProjectTo<GpuSeriesDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return null;

        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
