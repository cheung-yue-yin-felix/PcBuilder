using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpusHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetGpusQuery, PagedResult<GpuDto>>
{
    public Task<PagedResult<GpuDto>> Handle(GetGpusQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Gpus.List(
            request.PageIndex,
            request.PageSize,
            request.Name,
            request.ManufacturerId,
            request.GpuSeriesId);

        return cache.GetOrSetAsync(
            key,
            ct => context.Gpus
                .AsNoTracking()
                .Where(x => x.IsActive)
                .WhereIf(request.ManufacturerId.HasValue, x => x.ManufacturerId == request.ManufacturerId)
                .WhereIf(request.GpuSeriesId.HasValue, x => x.SeriesId == request.GpuSeriesId)
                .WhereIf(!string.IsNullOrEmpty(request.Name), x => x.Name.Contains(request.Name!))
                .ToPagedResultAsync<Gpu, GpuDto>(
                    request.PageIndex,
                    request.PageSize,
                    mapper.ConfigurationProvider,
                    ct),
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
