using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public class GetManufacturersHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetManufacturersQuery, List<ManufacturerDto>>
{
    public Task<List<ManufacturerDto>> Handle(GetManufacturersQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.Manufacturers.All(),
            async ct =>
            {
                var entities = await context.Manufacturers
                    .AsNoTracking()
                    .Where(m => m.IsActive)
                    .ToListAsync(ct);

                return mapper.Map<List<ManufacturerDto>>(entities);
            },
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
