using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public class GetChipsetsHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetChipsetsQuery, List<ChipsetDto>>
{
    public Task<List<ChipsetDto>> Handle(GetChipsetsQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.Chipsets.All(),
            ct => context.Chipsets.AsNoTracking()
                .Include(c => c.Manufacturer)
                .Include(c => c.Socket)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ProjectTo<ChipsetDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct),
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
