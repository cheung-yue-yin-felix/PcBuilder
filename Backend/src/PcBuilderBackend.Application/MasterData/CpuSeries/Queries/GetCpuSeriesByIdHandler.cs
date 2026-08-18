using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public class GetCpuSeriesByIdHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetCpuSeriesByIdQuery, CpuSeriesDto?>
{
    public async Task<CpuSeriesDto?> Handle(GetCpuSeriesByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.CpuSeries.ById(request.CpuSeriesId);
        var cached = await cache.GetAsync<CpuSeriesDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var result = await context.CpuSeries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CpuSeriesId && x.IsActive, cancellationToken);

        if (result is null)
            return null;

        var dto = mapper.Map<CpuSeriesDto>(result);
        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
