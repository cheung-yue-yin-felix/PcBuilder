using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpuByIdHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetGpuByIdQuery, GpuDto?>
{
    public async Task<GpuDto?> Handle(GetGpuByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Gpus.ById(request.Id);
        var cached = await cache.GetAsync<GpuDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await context.Gpus
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<GpuDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return null;

        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
