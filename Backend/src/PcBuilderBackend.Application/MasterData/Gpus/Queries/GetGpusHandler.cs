using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpusHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetGpusQuery, List<GpuDto>>
{
    public Task<List<GpuDto>> Handle(GetGpusQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Gpus.All();

        return cache.GetOrSetAsync(
            key,
            ct => context.Gpus
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => mapper.Map<GpuDto>(x))
                .ToListAsync(ct),
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
